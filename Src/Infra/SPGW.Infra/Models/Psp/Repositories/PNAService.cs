using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.DTOs;
using SPGW.Domain.Order.Entities;
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
using TechnoPaymentWebServiceService;

namespace SPGW.Infra.Models.Psp.Repositories
{
    public class PNAService //: IPNAService
    {
        //public const string RedirectAddress = "https://sepandpay.ir/GW/PNABack";
        ////public const string RedirectAddress = "http://localhost:23000/GW/PNABack";

        //public readonly string PSPPassword;
        //public static TechnoIPGWSClient _client = new TechnoIPGWSClient();//TechnoPaymentWebServiceService

        //private IEFRepository<Customer> _customerRepository;
        //private IEFRepository<Order> _orderRepository;
        //private IEFRepository<Domain.Psp.Entities.Psp> _pspRepository;
        //private IEFRepository<PspRequest> _pSPRequestRepository;
        //private IEFRepository<Transaction> _transactionRepository;
        //public PNAService(
        //    IEFRepository<Customer> customerRepository,
        //    IEFRepository<Order> orderRepository,
        //    IEFRepository<Domain.Psp.Entities.Psp> pspRepository,
        //    IEFRepository<PspRequest> pSPRequestRepository,
        //    IEFRepository<Transaction> transactionRepository
        //    )
        //{
        //    _customerRepository = customerRepository;
        //    _orderRepository = orderRepository;
        //    _pspRepository = pspRepository;
        //    _pSPRequestRepository = pSPRequestRepository;
        //    _transactionRepository = transactionRepository;

        //    var pnaPSP = _pspRepository.Find((long)PSPs.PNA);
        //    PSPPassword = pnaPSP.Password;

        //    //_client = new TechnoPaymentWebServiceService();
        //}


        //public ServiceResult<WSContext> Login(string username)
        //{
        //    return Login(username, PSPPassword);
        //}
        //public async ServiceResult<WSContext> Login(string username, string password)
        //{
        //    var result = new ServiceResult<WSContext>();
        //    try
        //    {
        //        //Step1----> Login
        //        var loginParam = new LoginParam
        //        {
        //            UserName = username,
        //            Password = password
        //        };                
        //        //ipgPage.Timeout = 150000;
        //        var loginResult = _client.MerchantLoginAsync(loginParam);
        //        if (loginResult != null)
        //        {
        //            if (loginResult.Result == "erSucceed")
        //            {
        //                var wsContext = new WSContext
        //                {
        //                    SessionId = loginResult.SessionId,
        //                    UserId = username,
        //                    Password = password
        //                };
        //                result.Result = wsContext;
        //            }
        //            else
        //            {
        //                result.ErrorCode = 2;
        //                result.ErrorMessage = "خطا در ورود  ";// + loginResult.Result;
        //            }
        //        }
        //        else
        //        {
        //            result.ErrorCode = 1;
        //            result.ErrorMessage = "عدم دریافت نتیجه login از پرداخت نوین آرین";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = "خطای نا شناخته";
        //    }
        //    return result;
        //}

        //public ServiceResult<object> GetPSPDetails(string orderCode, string payerName, string payerMobile)
        //{
        //    var result = new ServiceResult<object>();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(orderCode))
        //        {
        //            result.ErrorCode = 7;
        //            result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
        //        }
        //        else
        //        {
        //            var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => o.OrderCode == orderCode);
        //            if (currentOrder == null)
        //            {
        //                result.ErrorCode = 7;
        //                result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
        //            }
        //            else
        //            {
        //                if (!currentOrder.Customer.IsActive)
        //                {
        //                    result.ErrorCode = 8;
        //                    result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " غیر فعال است. لطفا با پشتیبانی تماس بگیرید";
        //                }
        //                else
        //                {
        //                    var merchants = currentOrder.Customer.Merchants.Where(m => m.IsActive);
        //                    if (!merchants.Any())
        //                    {
        //                        result.ErrorCode = 9;
        //                        result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ پذیرنده فعالی ندارد. لطفا با پشتیبانی تماس بگیرید";
        //                    }
        //                    else
        //                    {
        //                        var psps = merchants.Where(m => m.PSP.IsActive);
        //                        if (!psps.Any(p => p.PSPId == (long)PSPs.PNA))
        //                        {
        //                            result.ErrorCode = 11;
        //                            result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ درگاه فعال پرداخت نوینی ندارد. لطفا با پشتیبانی تماس بگیرید";
        //                        }
        //                        else
        //                        {
        //                            var currentTransaction = _transactionRepository.Queryable().OrderByDescending(t => t.Id).FirstOrDefault(t => t.OrderId == currentOrder.Id);
        //                            if (currentTransaction != null && currentTransaction.Status != (int)TransactionStates.Register)
        //                            {
        //                                result.Result = new PSPDetailResult
        //                                {
        //                                    RedirectToMerchant = true,
        //                                    OrderCode = currentOrder.OrderCode,
        //                                    redirectAddress = currentOrder.RedirectAddress
        //                                };
        //                            }
        //                            var merchant = currentOrder.Customer.Merchants.FirstOrDefault(m => m.PSPId == (long)PSPs.PNA);
        //                            var now = DateTime.Now;

        //                            //====================================================================================================
        //                            var loginResult = this.Login(merchant.MerchantCode, PSPPassword);

        //                            _pSPRequestRepository.Insert(new PSPRequest
        //                            {
        //                                OrderId = currentOrder.Id,
        //                                PSPId = merchant.PSPId,
        //                                Request = JsonConvert.SerializeObject(new { MerchantCode = merchant.MerchantCode, PSPPassword = PSPPassword }),
        //                                Response = JsonConvert.SerializeObject(loginResult),
        //                            });

        //                            if (loginResult.ErrorCode != 0)
        //                            {
        //                                result.ErrorCode = 12;
        //                                result.ErrorMessage = "خطا در ورود به سرویس پرداخت نوین - عدم احراز هویت";
        //                            }
        //                            else
        //                            {
        //                                var invoice = new RequestParam
        //                                {
        //                                    WSContext = loginResult.Result,
        //                                    TransType = enTransType.enGoods,
        //                                    TransTypeSpecified = true,
        //                                    ReserveNum = DateTime.Now.Ticks.ToString(),
        //                                    TerminalId = merchant.TerminalCode,
        //                                    Amount = Convert.ToDecimal(currentOrder.Amount),
        //                                    RedirectUrl = RedirectAddress,
        //                                    MerchantId = merchant.MerchantCode,
        //                                    AmountSpecified = true
        //                                };

        //                                var generateTransactionDataToSignResult = _client.GenerateTransactionDataToSignAsync(invoice);

        //                                _pSPRequestRepository.Insert(new PspRequest
        //                                {
        //                                    OrderId = currentOrder.Id,
        //                                    PSPId = merchant.PSPId,
        //                                    Request = JsonConvert.SerializeObject(invoice),
        //                                    Response = JsonConvert.SerializeObject(generateTransactionDataToSignResult),
        //                                });

        //                                if (generateTransactionDataToSignResult.Result == "erSucceed")
        //                                {
        //                                    var dataToSign = generateTransactionDataToSignResult.DataToSign;
        //                                    var uniqueId = generateTransactionDataToSignResult.UniqueId;

        //                                    //Step3----> Sign
        //                                    //var sign = new Sign();
        //                                    //var signedBytes = sign.SignSomeText(dataToSign);
        //                                    //var signedText = Convert.ToBase64String(signedBytes);
        //                                    //Step4----> GenerateSignedDataTokenIpgTest.GenerateSignedDataTokenParam generateSignedDataTokenParam = new GenerateSignedDataTokenParam();
        //                                    var tokenParams = new GenerateSignedDataTokenParam
        //                                    {
        //                                        //Signature = signedText,
        //                                        Signature = dataToSign,
        //                                        UniqueId = uniqueId,
        //                                        WSContext = loginResult.Result
        //                                    };
        //                                    var tokenResult = _client.GenerateSignedDataToken(tokenParams);

        //                                    _pSPRequestRepository.Insert(new PSPRequest
        //                                    {
        //                                        OrderId = currentOrder.Id,
        //                                        PSPId = merchant.PSPId,
        //                                        Request = JsonConvert.SerializeObject(tokenParams),
        //                                        Response = JsonConvert.SerializeObject(tokenResult),
        //                                    });

        //                                    if (tokenResult.Token != null)
        //                                    {
        //                                        if (currentTransaction != null)
        //                                        {
        //                                            currentTransaction.PSPId = merchant.PSPId;

        //                                            //PEP
        //                                            currentTransaction.PaymentDate = now;
        //                                            currentTransaction.InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss");
        //                                            currentTransaction.InvoiceNumber = currentOrder.InvoiceNumber.ToString();

        //                                            //PNA
        //                                            currentTransaction.ReserveNumber = invoice.ReserveNum;
        //                                            currentTransaction.Token = tokenResult.Token;

        //                                            //SPGW
        //                                            currentTransaction.PayerName = payerName;
        //                                            currentTransaction.PayerMobileNumber = payerMobile;

        //                                            _transactionRepository.Update(currentTransaction);
        //                                        }
        //                                        else
        //                                        {
        //                                            _transactionRepository.Insert(new Transaction
        //                                            {
        //                                                Amount = currentOrder.Amount,
        //                                                TotalAmount = currentOrder.TotalAmount,
        //                                                CustomerId = currentOrder.CustomerId,
        //                                                MerchantCode = merchant.MerchantCode,
        //                                                OrderId = currentOrder.Id,

        //                                                PaymentDate = now,
        //                                                PPaymentDate = int.Parse(now.ToPersianDateTimeString_PPAymentDate()),
        //                                                PaymentTime = now.TimeOfDay,//  int.Parse(now.ToPersianDateTimeString_Time()),

        //                                                PSPId = merchant.PSPId,
        //                                                Status = 0,
        //                                                RRN = "",

        //                                                //PEP
        //                                                InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss"),
        //                                                InvoiceNumber = currentOrder.InvoiceNumber.ToString(),
        //                                                TransactionRefrenceNumber = "",

        //                                                //PNA
        //                                                ReserveNumber = invoice.ReserveNum,
        //                                                Token = tokenResult.Token,

        //                                                //SPGW
        //                                                PayerName = payerName,
        //                                                PayerMobileNumber = payerMobile
        //                                            });
        //                                        }
        //                                        result.Result = new PSPDetailResult
        //                                        {
        //                                            RedirectToMerchant = false,
        //                                            OrderCode = tokenResult.Token,
        //                                            redirectAddress = ""
        //                                        };
        //                                    }
        //                                    else
        //                                    {
        //                                        result.ErrorCode = 14;
        //                                        result.ErrorMessage = "خطا در دریافت توکن در سمت پرداخت نوین";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    result.ErrorCode = 13;
        //                                    result.ErrorMessage = "خطا در ثبت سفارش در سمت پرداخت نوین";
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = ex.Message;
        //        throw;
        //    }
        //    return result;
        //}

        //public ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string ResNum, string token, string RefNum, decimal? MID)
        //{
        //    ServiceResult<RedirectToMerchantModel> result = new ServiceResult<RedirectToMerchantModel>();
        //    try
        //    {
        //        var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.Token == token);
        //        if (currentTransaction == null)
        //        {
        //            result.ErrorCode = 12;
        //            result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
        //        }
        //        else
        //        {
        //            if (currentTransaction.Status == 0)
        //            {
        //                var loginresult = this.Login(currentTransaction.MerchantCode, PSPPassword);
        //                if (loginresult.ErrorCode == 0)
        //                {

        //                    var getinfoparam = new TransReportParam
        //                    {
        //                        WSContext = loginresult.Result,
        //                        reserveNumber = ResNum
        //                    };
        //                    var resultinfo = _client.getTransactionReport(getinfoparam);

        //                    _pSPRequestRepository.Insert(new PSPRequest
        //                    {
        //                        OrderId = currentTransaction.OrderId,
        //                        PSPId = (long)PSPs.PNA,
        //                        Request = JsonConvert.SerializeObject(getinfoparam),
        //                        Response = JsonConvert.SerializeObject(resultinfo),
        //                    });

        //                    if (resultinfo.result == 0)
        //                    {
        //                        if (resultinfo.transactionList != null)
        //                        {
        //                            var ctr = resultinfo.transactionList.LastOrDefault();
        //                            currentTransaction.TransactionRefrenceNumber = ctr.switchRefCode;
        //                            currentTransaction.ReferenceNumber = RefNum;
        //                            currentTransaction.MID = MID.ToString();
        //                            currentTransaction.MaskedCarNumber = ctr.maskPan;
        //                            currentTransaction.RRN = ctr.Rrn;
        //                            if (ctr.decideReqType == null)
        //                            {
        //                                if (ctr.ResponseCode == 0)
        //                                {
        //                                    currentTransaction.Status = 2;
        //                                }
        //                                else
        //                                {
        //                                    currentTransaction.Status = 1;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (ctr.decideReqType == "R")
        //                                {
        //                                    currentTransaction.Status = 3;
        //                                }
        //                                else if (ctr.decideReqType == "V")
        //                                {
        //                                    currentTransaction.Status = 4;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            currentTransaction.Status = 1;
        //                        }
        //                        _transactionRepository.Update(currentTransaction);
        //                        result.Result = new RedirectToMerchantModel
        //                        {
        //                            OrderCode = currentTransaction.Order.OrderCode,
        //                            redirectAddress = currentTransaction.Order.RedirectAddress
        //                        };
        //                    }
        //                    else
        //                    {
        //                        result.ErrorCode = 21;
        //                        result.ErrorMessage = "خطا در دریافت اطلاعات مربوط به تراکنش از پرداخت نوین";
        //                    }
        //                }
        //                else
        //                {
        //                    result.ErrorCode = loginresult.ErrorCode;
        //                    result.ErrorMessage = loginresult.ErrorMessage;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = "خطای نا شناخته";
        //        result.Result = null;
        //    }
        //    return result;
        //}

        //public ServiceResult<bool> CheckTransactionResult(string ResNum)
        //{
        //    ServiceResult<bool> result = new ServiceResult<bool>();
        //    try
        //    {
        //        var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.ReserveNumber == ResNum);
        //        if (currentTransaction == null)
        //        {
        //            result.ErrorCode = 12;
        //            result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
        //        }
        //        else
        //        {
        //            if (currentTransaction.Status == 0)
        //            {
        //                var loginresult = this.Login(currentTransaction.MerchantCode, PSPPassword);
        //                if (loginresult.ErrorCode == 0)
        //                {

        //                    var getinfoparam = new TransReportParam
        //                    {
        //                        WSContext = loginresult.Result,
        //                        reserveNumber = ResNum
        //                    };
        //                    var resultinfo = _client.getTransactionReport(getinfoparam);

        //                    _pSPRequestRepository.Insert(new PSPRequest
        //                    {
        //                        OrderId = currentTransaction.OrderId,
        //                        PSPId = (long)PSPs.PNA,
        //                        Request = JsonConvert.SerializeObject(getinfoparam),
        //                        Response = JsonConvert.SerializeObject(resultinfo),
        //                    });

        //                    if (resultinfo.result == 0)
        //                    {
        //                        if (resultinfo.transactionList != null)
        //                        {
        //                            var ctr = resultinfo.transactionList.LastOrDefault();
        //                            currentTransaction.TransactionRefrenceNumber = ctr.switchRefCode;
        //                            //currentTransaction.ReferenceNumber = RefNum;
        //                            //currentTransaction.MID = MID.ToString();
        //                            currentTransaction.MaskedCarNumber = ctr.maskPan;
        //                            currentTransaction.RRN = ctr.Rrn;
        //                            if (ctr.decideReqType == null)
        //                            {
        //                                currentTransaction.Status = 2;
        //                            }
        //                            else
        //                            {
        //                                if (ctr.decideReqType == "R")
        //                                {
        //                                    currentTransaction.Status = 3;
        //                                }
        //                                else if (ctr.decideReqType == "V")
        //                                {
        //                                    currentTransaction.Status = 4;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            currentTransaction.Status = 1;
        //                        }
        //                        _transactionRepository.Update(currentTransaction);
        //                        result.Result = true;
        //                    }
        //                    else
        //                    {
        //                        result.ErrorCode = 21;
        //                        result.ErrorMessage = "خطا در دریافت اطلاعات مربوط به تراکنش از پرداخت نوین";
        //                    }
        //                }
        //                else
        //                {
        //                    result.ErrorCode = loginresult.ErrorCode;
        //                    result.ErrorMessage = loginresult.ErrorMessage;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = "خطای نا شناخته";
        //        result.Result = false;
        //    }
        //    return result;
        //}

        //public ServiceResult<bool> VerifyPaymentMethod(Order order)
        //{
        //    ServiceResult<bool> result = new ServiceResult<bool>();

        //    var ltr = order.Transactions.OrderByDescending(t => t.Id).FirstOrDefault();

        //    var loginresult = Login(ltr.MerchantCode);
        //    if (loginresult.ErrorCode == 0)
        //    {
        //        var verifyParam = new VerifyMerchantTransParam { WSContext = loginresult.Result, Token = ltr.Token, RefNum = ltr.ReferenceNumber };
        //        var verifyResult = _client.VerifyMerchantTrans(verifyParam);

        //        _pSPRequestRepository.Insert(new PSPRequest
        //        {
        //            OrderId = ltr.OrderId,
        //            PSPId = (long)PSPs.PEP,
        //            Request = JsonConvert.SerializeObject(verifyParam),
        //            Response = JsonConvert.SerializeObject(verifyResult),
        //        });

        //        if (verifyResult.Result.Equals("erSucceed"))
        //        {
        //            ltr.Status = 4;
        //            result.Result = true;
        //            _transactionRepository.Update(ltr);
        //        }
        //        else
        //        {
        //            result.Result = false;
        //        }
        //    }
        //    else
        //    {
        //        result.ErrorMessage = loginresult.ErrorMessage;
        //        result.ErrorCode = loginresult.ErrorCode;
        //    }
        //    return result;
        //}

        //public ServiceResult<bool> RefoundPaymentMethod(Order order)
        //{
        //    ServiceResult<bool> result = new ServiceResult<bool>();

        //    var ltr = order.Transactions.OrderByDescending(t => t.Id).FirstOrDefault();

        //    var loginresult = Login(ltr.MerchantCode);
        //    if (loginresult.ErrorCode == 0)
        //    {
        //        var reverseParam = new ReverseMerchantTransParam { WSContext = loginresult.Result, Token = ltr.Token, RefNum = ltr.ReferenceNumber };
        //        var reverseResult = _client.ReverseMerchantTrans(reverseParam);

        //        _pSPRequestRepository.Insert(new PSPRequest
        //        {
        //            OrderId = ltr.OrderId,
        //            PSPId = (long)PSPs.PEP,
        //            Request = JsonConvert.SerializeObject(reverseParam),
        //            Response = JsonConvert.SerializeObject(reverseResult),
        //        });

        //        if (reverseResult.Result.Equals("erSucceed"))
        //        {
        //            ltr.Status = 3;
        //            result.Result = true;
        //            _transactionRepository.Update(ltr);
        //        }
        //        else
        //        {
        //            result.Result = false;
        //        }
        //    }
        //    else
        //    {
        //        result.ErrorMessage = loginresult.ErrorMessage;
        //        result.ErrorCode = loginresult.ErrorCode;
        //    }
        //    return result;
        //}
    }
}
