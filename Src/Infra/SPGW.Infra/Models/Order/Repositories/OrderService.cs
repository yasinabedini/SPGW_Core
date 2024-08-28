using Newtonsoft.Json;
using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.DTOs;
using SPGW.Domain.Order.Repositories;
using SPGW.Domain.Psp.Entities;
using SPGW.Domain.Psp.Enums;
using SPGW.Domain.Psp.Repositories;
using SPGW.Domain.Transaction.Entities;
using SPGW.Domain.Transaction.Enums;
using SPGW.Infra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Models.Order.Repositories
{
    public class OrderService : IOrderService
    {
        private IPEPService _pepService;
        private IPNAService _pnaService;
        private IIRKService _irkService;
        private IBPMService _bpmService;

        private IEFRepository<Domain.Customer.Entities.Customer> _customerRepository;
        private IEFRepository<Domain.Order.Entities.Order> _orderRepository;
        private IEFRepository<PspRequest> _pSPRequestRepository;
        private IEFRepository<Transaction> _transactionRepository;
        public OrderService(
            IEFRepository<Domain.Customer.Entities.Customer> customerRepository,
            IEFRepository<Domain.Order.Entities.Order> orderRepository,
            IPEPService pepService,
            IPNAService pnaService,
            IEFRepository<PspRequest> pSPRequestRepository,
            IEFRepository<Transaction> transactionRepository,
            IIRKService irkService,
            IBPMService bpmService
            )
        {
            _pepService = pepService;
            _pnaService = pnaService;
            _irkService = irkService;
            _bpmService = bpmService;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _pSPRequestRepository = pSPRequestRepository;
            _transactionRepository = transactionRepository;
        }

        public ServiceResult<OrderDetails> GetOrderDetails(OrderModel order)
        {
            var result = new ServiceResult<OrderDetails>();
            try
            {
                var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => (o.OrderCode == order.OrderCode) || (o.InvoiceDate == order.InvoiceDate && o.InvoiceNumber == order.InvoiceNumber));
                if (currentOrder == null)
                {
                    result.ErrorCode = 13;
                    result.ErrorMessage = "فاکتوری  با این مشخصات ثبت نشده است ";
                }
                else
                {
                    result.Result = new OrderDetails
                    {
                        Amount = currentOrder.Amount,
                        CustomerCode = currentOrder.Customer.CustomerCode,
                        InvoiceDate = currentOrder.InvoiceDate,
                        InvoiceNumber = currentOrder.InvoiceNumber,
                        OrderCode = currentOrder.OrderCode,
                        RedirectAddress = currentOrder.RedirectAddress,
                        TotalAmount = currentOrder.TotalAmount,
                        DirectPSPGW = currentOrder.DirectPSPGW,
                        PSPCode = currentOrder.PSPCode
                    };

                    var currentTransaction = currentOrder.Transactions.FirstOrDefault();
                    if (currentTransaction != null)
                    {
                        if (currentTransaction.Status == (int)TransactionStates.Register)
                        {
                            switch ((PSPs)currentTransaction.PSPId)
                            {
                                case PSPs.PEP:

                                    PEPCheckTransactionResult checkResult;
                                    if (string.IsNullOrEmpty(currentTransaction.TransactionRefrenceNumber))
                                    {
                                        checkResult = _pepService.CallCheckTransactionResultMethodByiNiD(
                                                currentTransaction.InvoiceNumber,
                                        currentTransaction.InvoiceDateString,
                                                currentOrder.Customer.Merchants.FirstOrDefault().TerminalCode,
                                                currentTransaction.MerchantCode
                                            );

                                        _pSPRequestRepository.Insert(new PspRequest
                                        {
                                            OrderId = currentTransaction.OrderId,
                                            PSPId = (long)PSPs.PEP,
                                            Request = JsonConvert.SerializeObject(new
                                            {
                                                InvoiceNumber = currentTransaction.InvoiceNumber,
                                                InvoiceDate = currentTransaction.InvoiceDateString,
                                                TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault().TerminalCode,
                                                MerchantCode = currentTransaction.MerchantCode
                                            }),
                                            Response = JsonConvert.SerializeObject(checkResult),
                                        });
                                    }
                                    else
                                    {
                                        checkResult = _pepService.CallCheckTransactionResultMethodByTref(currentTransaction.TransactionRefrenceNumber);

                                        _pSPRequestRepository.Insert(new PspRequest
                                        {
                                            OrderId = currentTransaction.OrderId,
                                            PSPId = (long)PSPs.PEP,
                                            Request = JsonConvert.SerializeObject(new
                                            {
                                                TransactionRefrenceNumber = currentTransaction.TransactionRefrenceNumber
                                            }),
                                            Response = JsonConvert.SerializeObject(checkResult),
                                        });
                                    }
                                    if (!checkResult.IsSuccess)
                                    {
                                        result.Result.Status = currentTransaction.Status = 1;
                                    }
                                    else
                                    {
                                        result.Result.PaymentDate = currentTransaction.PaymentDate != null ? ((DateTime)currentTransaction.PaymentDate).ToString("yyyy/MM/dd HH:mm:ss") : "";
                                        result.Result.TransactionRefrenceNumber = currentTransaction.TransactionRefrenceNumber = checkResult.TransactionReferenceID;
                                        currentTransaction.ReferenceNumber = checkResult.ReferenceNumber.ToString();
                                        result.Result.Status = currentTransaction.Status = 2;
                                        result.Result.RRN = currentTransaction.RRN;
                                        result.Result.MaskedPan = currentTransaction.MaskedCarNumber;
                                    }
                                    _transactionRepository.Update(currentTransaction);
                                    break;

                                case PSPs.PNA:

                                    var checkresult = _pnaService.CheckTransactionResult(currentTransaction.ReserveNumber);

                                    if (checkresult.ErrorCode != 0)
                                    {
                                        result.ErrorCode = 26;
                                        result.ErrorMessage = "خطا در واکشی اطلاعات از پرداخت نوین";
                                    }
                                    else
                                    {
                                        currentTransaction = currentOrder.Transactions.FirstOrDefault();
                                    }
                                    break;

                                case PSPs.IRK:

                                    RedirectToMerchantModel irkCheckResult = new RedirectToMerchantModel();
                                    if (string.IsNullOrEmpty(currentTransaction.TransactionRefrenceNumber))
                                    {
                                        irkCheckResult = _irkService.CheckTransactionResult(
                                                currentTransaction.Token,
                                                currentTransaction.ResponseCode,
                                                currentTransaction.Id.ToString(),
                                                currentOrder.Id.ToString(),
                                                currentTransaction.RRN,
                                                currentTransaction.SystemTraceAuditNumber
                                            ).Result;

                                        _pSPRequestRepository.Insert(new PspRequest
                                        {
                                            OrderId = currentTransaction.OrderId,
                                            PSPId = (long)PSPs.IRK,
                                            Request = JsonConvert.SerializeObject(new
                                            {
                                                InvoiceNumber = currentTransaction.InvoiceNumber,
                                                InvoiceDate = currentTransaction.InvoiceDateString,
                                                TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault().TerminalCode,
                                                MerchantCode = currentTransaction.MerchantCode
                                            }),
                                            Response = JsonConvert.SerializeObject(irkCheckResult),
                                        });
                                    }
                                    else
                                    {
                                        irkCheckResult = _irkService.CheckTransactionResult(
                                            currentTransaction.Token,
                                            currentTransaction.ResponseCode,
                                            currentTransaction.Id.ToString(),
                                            currentOrder.Id.ToString(),
                                            currentTransaction.RRN,
                                            currentTransaction.SystemTraceAuditNumber
                                        ).Result;

                                        _pSPRequestRepository.Insert(new PspRequest
                                        {
                                            OrderId = currentTransaction.OrderId,
                                            PSPId = (long)PSPs.IRK,
                                            Request = JsonConvert.SerializeObject(new
                                            {
                                                TransactionRefrenceNumber = currentTransaction.TransactionRefrenceNumber
                                            }),
                                            Response = JsonConvert.SerializeObject(irkCheckResult),
                                        });
                                    }
                                    if (!irkCheckResult.IsSuccessful)
                                    {
                                        result.Result.Status = currentTransaction.Status = 1;
                                    }
                                    else
                                    {
                                        result.Result.PaymentDate = currentTransaction.PaymentDate != null ? ((DateTime)currentTransaction.PaymentDate).ToString("yyyy/MM/dd HH:mm:ss") : "";
                                        //result.Result.TransactionRefrenceNumber = currentTransaction.TransactionRefrenceNumber = irkCheckResult.TransactionReferenceID;
                                        //currentTransaction.ReferenceNumber = irkCheckResult.ReferenceNumber.ToString();
                                        result.Result.Status = currentTransaction.Status = 2;
                                        result.Result.RRN = currentTransaction.RRN;
                                        result.Result.MaskedPan = currentTransaction.MaskedCarNumber;
                                    }
                                    _transactionRepository.Update(currentTransaction);
                                    break;
                                case PSPs.BPM:
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            result.Result.PaymentDate = currentTransaction.PaymentDate != null ? ((DateTime)currentTransaction.PaymentDate).ToString("yyyy/MM/dd HH:mm:ss") : "";
                            result.Result.Status = currentTransaction.Status;
                            result.Result.RRN = currentTransaction.RRN;
                            result.Result.TransactionRefrenceNumber = currentTransaction.TransactionRefrenceNumber;
                            result.Result.MaskedPan = currentTransaction.MaskedCarNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 98;
                result.ErrorMessage = "خطا در واکشی اطلاعات";
            }
            return result;
        }

        public ServiceResult<string> Register(OrderModel order)
        {
            var result = new ServiceResult<string>();

            if (string.IsNullOrEmpty(order.CustomerCode))
            {
                result.ErrorCode = 1;
                result.ErrorMessage = "کد مشتری حتما باید مقدار داشته باشد";
            }

            if (order.Amount <= 0)
            {
                result.ErrorCode = 1;
                result.ErrorMessage = "مقدار مبلغ باید صحیح و بیشتر از صفر باشد";
            }

            if (order.TotalAmount <= 0)
            {
                result.ErrorCode = 2;
                result.ErrorMessage = "مقدار مبلغ کل باید صحیح و بیشتر از صفر باشد";
            }

            if (order.TotalAmount < order.Amount)
            {
                result.ErrorCode = 3;
                result.ErrorMessage = "مقدار مبلغ کل باید بیشتر یا مساوی مبلغ قابل پرداخت باشد";
            }

            if (string.IsNullOrEmpty(order.RedirectAddress))
            {
                result.ErrorCode = 4;
                result.ErrorMessage = "آدرس برگشت حتما باید مقدار داشته باشد";
            }

            if (order.InvoiceNumber <= 0)
            {
                result.ErrorCode = 5;
                result.ErrorMessage = "شماره فاکتور باید صحیح و بیشتر از صفر باشد";
            }

            if (order.DirectPSPGW == true)
            {
                if (order.PSPCode == 0)
                {
                    result.ErrorCode = 7;
                    result.ErrorMessage = "در صورتی که انتقال مستقیم به درگاه بانک فعال باشد PSPCode باید مقدار دهی شود";
                }
                else
                {
                    try
                    {
                        var psp = (PSPs)order.PSPCode;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCode = 8;
                        result.ErrorMessage = "PSPCode داده شده معتبر نمی باشد";
                    }
                }
            }


            if (result.ErrorCode == 0)
            {
                var currentCustomer = _customerRepository.Queryable().FirstOrDefault(c => c.IsActive && c.CustomerCode == order.CustomerCode);
                if (currentCustomer == null)
                {
                    result.ErrorCode = 6;
                    result.ErrorMessage = "کد مشتری داده شده نا معتبر است یا مسدود شده است";
                }
                else
                {
                    try
                    {
                        var todayStart = DateTime.Now.StartOfDay();
                        var todayEnd = DateTime.Now.EndOfDay();

                        int orderCount = _orderRepository.Queryable().Count(o => o.CustomerId == currentCustomer.Id && o.RegisterDate >= todayStart && o.RegisterDate <= todayEnd);
                        orderCount++;
                        string orderCode = order.CustomerCode + order.Amount.ToString() + DateTime.Now.Ticks.ToString() + orderCount;


                        var currentOrder = new Domain.Order.Entities.Order
                        {
                            Amount = order.Amount,
                            CustomerId = currentCustomer.Id,
                            InvoiceDate = order.InvoiceDate,
                            InvoiceNumber = order.InvoiceNumber,
                            OrderCode = orderCode,
                            RedirectAddress = order.RedirectAddress,
                            TotalAmount = order.TotalAmount,
                            RegisterDate = DateTime.Now
                        };

                        if (order.DirectPSPGW == true)
                        {
                            currentOrder.DirectPSPGW = true;
                            currentOrder.PSPCode = order.PSPCode;
                        }

                        _orderRepository.Insert(currentOrder);

                        result.Result = orderCode;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCode = 99;
                        result.ErrorMessage = ex.Message;
                        throw;
                    }
                }
            }
            return result;
        }

        public ServiceResult<string> VerifyPayment(OrderModel order)
        {
            var result = new ServiceResult<string>();
            try
            {
                var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => (o.OrderCode == order.OrderCode) || (o.InvoiceDate == order.InvoiceDate && o.InvoiceNumber == order.InvoiceNumber));
                if (currentOrder == null)
                {
                    result.ErrorCode = 13;
                    result.ErrorMessage = "فاکتوری  با این مشخصات ثبت نشده است ";
                }
                else
                {
                    var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.OrderId == currentOrder.Id && t.Status == (int)TransactionStates.PaymentSuccess);
                    if (currentTransaction == null)
                    {
                        result.ErrorCode = 14;
                        result.ErrorMessage = "برای فاکتور مذکور تراکنش موفقی که قابل تایید باشد ثبت نشده است";
                    }
                    else
                    {
                        switch ((PSPs)currentTransaction.PSPId)
                        {
                            case PSPs.PEP:

                                var param = new PEPVerifyPaymentParamenterModel
                                {
                                    Amount = currentTransaction.Amount,
                                    InvoiceDate = currentTransaction.InvoiceDateString,
                                    InvoiceNumber = currentTransaction.InvoiceNumber,
                                    MerchantCode = currentTransaction.MerchantCode,
                                    TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault(t => t.MerchantCode == currentTransaction.MerchantCode).TerminalCode,
                                    TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")//((DateTime)currentTransaction.PaymentDate).ToString("yyyy/MM/dd HH:mm:ss")
                                };
                                var verifyResult = _pepService.CallVerifyPaymentMethod(param);

                                _pSPRequestRepository.Insert(new PspRequest
                                {
                                    OrderId = currentTransaction.OrderId,
                                    PSPId = (long)PSPs.PEP,
                                    Request = JsonConvert.SerializeObject(param),
                                    Response = JsonConvert.SerializeObject(verifyResult),
                                });

                                if (!verifyResult.IsSuccess)
                                {
                                    result.ErrorCode = 15;
                                    result.ErrorMessage = verifyResult.Message;
                                }
                                else
                                {
                                    currentTransaction.Status = 4;
                                    currentTransaction.MaskedCarNumber = verifyResult.MaskedCardNumber;
                                    result.Result = currentTransaction.RRN = verifyResult.ShaparakRefNumber;
                                }
                                _transactionRepository.Update(currentTransaction);
                                break;

                            case PSPs.PNA:

                                var pnaverifyResult = _pnaService.VerifyPaymentMethod(currentOrder);

                                if (pnaverifyResult.ErrorCode != 0)
                                {
                                    result.ErrorCode = 15;
                                    result.ErrorMessage = pnaverifyResult.ErrorMessage;
                                }
                                else
                                {
                                    result.Result = currentTransaction.RRN;
                                }
                                break;
                            case PSPs.IRK:
                                var irkParam = new IRKVerifyPaymentParamenterModel
                                {
                                    TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId).TerminalCode,
                                    ResponseCode = currentTransaction.ResponseCode,
                                    RRN = currentTransaction.RRN,
                                    Token = currentTransaction.Token,
                                    TraceNo = currentTransaction.SystemTraceAuditNumber// currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId).TraceNo
                                };
                                var irkVerifyResult = _irkService.CallVerifyPaymentMethod(irkParam);

                                _pSPRequestRepository.Insert(new PspRequest
                                {
                                    OrderId = currentTransaction.OrderId,
                                    PSPId = (long)PSPs.IRK,
                                    Request = JsonConvert.SerializeObject(irkParam),
                                    Response = JsonConvert.SerializeObject(irkVerifyResult),
                                });

                                if (!irkVerifyResult.IsSuccess)
                                {
                                    result.ErrorCode = 15;
                                    result.ErrorMessage = irkVerifyResult.Message;
                                }
                                else
                                {
                                    currentTransaction.Status = 4;
                                    result.Result = currentTransaction.RRN;
                                }
                                _transactionRepository.Update(currentTransaction);
                                break;
                            case PSPs.BPM:

                                var bpmverifyResult = _bpmService.VerifyPaymentMethod(currentOrder);

                                if (bpmverifyResult.ErrorCode != 0)
                                {
                                    result.ErrorCode = 15;
                                    result.ErrorMessage = bpmverifyResult.ErrorMessage;
                                }
                                else
                                {
                                    result.Result = currentTransaction.RRN;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 98;
                result.ErrorMessage = "خطا در واکشی اطلاعات";
            }
            return result;
        }

        public ServiceResult<bool> RefoundPayment(OrderModel order)
        {
            var result = new ServiceResult<bool>();
            try
            {
                var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => (o.OrderCode == order.OrderCode) || (o.InvoiceDate == order.InvoiceDate && o.InvoiceNumber == order.InvoiceNumber));
                if (currentOrder == null)
                {
                    result.ErrorCode = 13;
                    result.ErrorMessage = "فاکتوری  با این مشخصات ثبت نشده است ";
                }
                else
                {
                    var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.OrderId == currentOrder.Id && t.Status == (int)TransactionStates.PaymentSuccess);
                    if (currentTransaction == null)
                    {
                        result.ErrorCode = 14;
                        result.ErrorMessage = "برای فاکتور مذکور تراکنش موفقی که قابل تایید باشد ثبت نشده است";
                    }
                    else
                    {
                        switch ((PSPs)currentTransaction.PSPId)
                        {
                            case PSPs.PEP:

                                var param = new PEPRefoundPaymentParamenterModel
                                {
                                    InvoiceDate = currentTransaction.InvoiceDateString,
                                    InvoiceNumber = currentTransaction.InvoiceNumber,
                                    MerchantCode = currentTransaction.MerchantCode,//  int.Parse(),
                                    TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault(t => t.MerchantCode == currentTransaction.MerchantCode).TerminalCode,//int.Parse(),
                                    TimeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")//((DateTime)currentTransaction.PaymentDate).ToString("yyyy/MM/dd HH:mm:ss")
                                };
                                var verifyResult = _pepService.CallRefoundPaymentMethod(param);

                                _pSPRequestRepository.Insert(new PspRequest
                                {
                                    OrderId = currentTransaction.OrderId,
                                    PSPId = (long)PSPs.PEP,
                                    Request = JsonConvert.SerializeObject(param),
                                    Response = JsonConvert.SerializeObject(verifyResult),
                                });

                                if (!verifyResult.IsSuccess)
                                {
                                    result.ErrorCode = 16;
                                    result.ErrorMessage = verifyResult.Message;
                                    result.Result = false;
                                }
                                else
                                {
                                    currentTransaction.Status = 3;
                                    result.Result = true;
                                }
                                _transactionRepository.Update(currentTransaction);
                                break;
                            case PSPs.PNA:

                                var pnarefoundResult = _pnaService.RefoundPaymentMethod(currentOrder);

                                if (pnarefoundResult.ErrorCode != 0)
                                {
                                    result.ErrorCode = 15;
                                    result.ErrorMessage = pnarefoundResult.ErrorMessage;
                                }
                                else
                                {
                                    result.Result = true;
                                }
                                break;
                            case PSPs.IRK:

                                var irkParam = new IRKRefoundPaymentParamenterModel
                                {
                                    TerminalCode = currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId).TerminalCode,
                                    PassPhrase = currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId).PassPhrase,
                                    Token = currentTransaction.Token,
                                    ResponseCode = currentTransaction.ResponseCode,
                                    TraceNo = currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId).TraceNo
                                };
                                var irkVerifyResult = _irkService.CallRefoundPaymentMethod(irkParam);

                                _pSPRequestRepository.Insert(new PspRequest
                                {
                                    OrderId = currentTransaction.OrderId,
                                    PSPId = (long)PSPs.PEP,
                                    Request = JsonConvert.SerializeObject(irkParam),
                                    Response = JsonConvert.SerializeObject(irkVerifyResult),
                                });

                                if (!irkVerifyResult.IsSuccess)
                                {
                                    result.ErrorCode = 16;
                                    result.ErrorMessage = irkVerifyResult.Message;
                                    result.Result = false;
                                }
                                else
                                {
                                    currentTransaction.Status = 3;
                                    result.Result = true;
                                }
                                _transactionRepository.Update(currentTransaction);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 98;
                result.ErrorMessage = "خطا در واکشی اطلاعات";
            }
            return result;
        }
    }
}
