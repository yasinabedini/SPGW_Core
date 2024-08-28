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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Models.Psp.Repositories
{
    public class BPMService //: IBPMService
    {
        ////public const string RedirectAddress = "https://sepandpay.ir/GW/PNABack";
        //public const string RedirectAddress = "http://pay.setarepay.ir/GW/BPMBack";
        ////public const string RedirectAddress = "http://localhost:21000/GW/BPMBack";

        //public readonly string PSPPassword;
        //public static PaymentGatewayImplService _client = new PaymentGatewayImplService();// TechnoPaymentWebServiceService();//TechnoPaymentWebServiceService

        //private IEFRepository<Customer> _customerRepository;
        //private IEFRepository<Order> _orderRepository;
        //private IEFRepository<Domain.Psp.Entities.Psp> _pspRepository;
        //private IEFRepository<PspRequest> _pSPRequestRepository;
        //private IEFRepository<Transaction> _transactionRepository;
        //public BPMService(
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
        //                        if (!psps.Any(p => p.PSPId == (long)PSPs.BPM))
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
        //                            var merchant = currentOrder.Customer.Merchants.FirstOrDefault(m => m.PSPId == (long)PSPs.BPM);
        //                            var now = DateTime.Now;

        //                            //====================================================================================================
        //                            //var loginResult = this.Login(merchant.MerchantCode, PSPPassword);

        //                            //_pSPRequestRepository.Insert(new PSPRequest
        //                            //{
        //                            //    OrderId = currentOrder.Id,
        //                            //    PSPId = merchant.PSPId,
        //                            //    Request = JsonConvert.SerializeObject(new { MerchantCode = merchant.MerchantCode, PSPPassword = PSPPassword }),
        //                            //    Response = JsonConvert.SerializeObject(loginResult),
        //                            //});

        //                            //if (loginResult.ErrorCode != 0)
        //                            //{
        //                            //    result.ErrorCode = 12;
        //                            //    result.ErrorMessage = "خطا در ورود به سرویس پرداخت نوین - عدم احراز هویت";
        //                            //}
        //                            //else
        //                            //{
        //                            //var invoice = new RequestParam
        //                            //{
        //                            //    WSContext = loginResult.Result,
        //                            //    TransType = enTransType.enGoods,
        //                            //    TransTypeSpecified = true,
        //                            //    ReserveNum = DateTime.Now.Ticks.ToString(),
        //                            //    TerminalId = merchant.TerminalCode,
        //                            //    Amount = Convert.ToDecimal(currentOrder.Amount),
        //                            //    RedirectUrl = RedirectAddress,
        //                            //    MerchantId = merchant.MerchantCode,
        //                            //    AmountSpecified = true
        //                            //};

        //                            //var generateTransactionDataToSignResult = _client.GenerateTransactionDataToSign(invoice);

        //                            var invoice = new BPMPayRequestModel
        //                            {
        //                                terminalId = long.Parse(merchant.TerminalCode), //6188355,
        //                                userName = "Gholi25",//merchant.UserName
        //                                userPassword = "98143544",//merchant.UserPassword
        //                                orderId = DateTime.Now.Ticks,//.ToString()//currentOrder.Id,
        //                                amount = currentOrder.Amount,
        //                                localDate = DateTime.Now.BPM_ToDateString(),
        //                                localTime = DateTime.Now.BPM_ToTimeString(),
        //                                additionalData = "",
        //                                callBackUrl = RedirectAddress,
        //                                payerId = "0"

        //                                //WSContext = loginResult.Result,
        //                                //TransType = enTransType.enGoods,
        //                                //TransTypeSpecified = true,
        //                                //ReserveNum = DateTime.Now.Ticks.ToString(),
        //                                //TerminalId = merchant.TerminalCode,
        //                                //Amount = Convert.ToDecimal(currentOrder.Amount),
        //                                //RedirectUrl = RedirectAddress,
        //                                //MerchantId = merchant.MerchantCode,
        //                                //AmountSpecified = true
        //                            };
        //                            var payRequestToken = new BPMPayResponseModel();
        //                            var payRequestTokenString = _client.bpPayRequest(
        //                                invoice.terminalId,
        //                                invoice.userName,
        //                                invoice.userPassword,
        //                                invoice.orderId,
        //                                invoice.amount,
        //                                invoice.localDate,
        //                                invoice.localTime,
        //                                invoice.additionalData,
        //                                invoice.callBackUrl,
        //                                invoice.payerId,
        //                                "",
        //                                "",
        //                                "",
        //                                "",
        //                                "");

        //                            if (payRequestTokenString.Contains(","))
        //                            {
        //                                var splitedData = payRequestTokenString.Split(',');
        //                                payRequestToken.ErrorCode = long.Parse(splitedData[0]);
        //                                if (splitedData.Length > 1)
        //                                {
        //                                    payRequestToken.RefId = splitedData[1];
        //                                }
        //                            }
        //                            else
        //                            {
        //                                payRequestToken.ErrorCode = 99;
        //                                payRequestToken.ErrorMessage = "خطا در ثبت سفارش در سمت به پرداخت" + "  --  " + payRequestTokenString;
        //                            }

        //                            _pSPRequestRepository.Insert(new PspRequest
        //                            {
        //                                OrderId = currentOrder.Id,
        //                                PSPId = merchant.PSPId,
        //                                Request = JsonConvert.SerializeObject(invoice),
        //                                Response = JsonConvert.SerializeObject(payRequestToken),
        //                            });

        //                            if (payRequestToken.ErrorCode == 0)
        //                            {
        //                                if (currentTransaction != null)
        //                                {
        //                                    currentTransaction.PSPId = merchant.PSPId;
        //                                    currentTransaction.TerminalId = merchant.TerminalCode;

        //                                    //PEP
        //                                    currentTransaction.PaymentDate = now;
        //                                    currentTransaction.InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss");
        //                                    currentTransaction.InvoiceNumber = currentOrder.InvoiceNumber.ToString();

        //                                    //PNA
        //                                    //currentTransaction.ReserveNumber = invoice.ReserveNum;
        //                                    //currentTransaction.Token = tokenResult.Token;

        //                                    //BPM
        //                                    currentTransaction.ReserveNumber = invoice.orderId.ToString();
        //                                    currentTransaction.Token = payRequestToken.RefId;

        //                                    //SPGW
        //                                    currentTransaction.PayerName = payerName;
        //                                    currentTransaction.PayerMobileNumber = payerMobile;

        //                                    _transactionRepository.Update(currentTransaction);
        //                                }
        //                                else
        //                                {
        //                                    _transactionRepository.Insert(new Transaction
        //                                    {
        //                                        Amount = currentOrder.Amount,
        //                                        TotalAmount = currentOrder.TotalAmount,
        //                                        CustomerId = currentOrder.CustomerId,
        //                                        MerchantCode = merchant.MerchantCode,
        //                                        OrderId = currentOrder.Id,
        //                                        TerminalId = merchant.TerminalCode,

        //                                        PaymentDate = now,
        //                                        PPaymentDate = int.Parse(now.ToPersianDateTimeString_PPAymentDate()),
        //                                        PaymentTime = now.TimeOfDay,//  int.Parse(now.ToPersianDateTimeString_Time()),

        //                                        PSPId = merchant.PSPId,
        //                                        Status = 0,
        //                                        RRN = "",

        //                                        //PEP
        //                                        InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss"),
        //                                        InvoiceNumber = currentOrder.InvoiceNumber.ToString(),
        //                                        TransactionRefrenceNumber = "",

        //                                        //PNA
        //                                        //ReserveNumber = invoice.ReserveNum,
        //                                        //Token = tokenResult.Token,

        //                                        //BPM
        //                                        ReserveNumber = invoice.orderId.ToString(),
        //                                        Token = payRequestToken.RefId,

        //                                        //SPGW
        //                                        PayerName = payerName,
        //                                        PayerMobileNumber = payerMobile
        //                                    });
        //                                }
        //                                result.Result = new PSPDetailResult
        //                                {
        //                                    RedirectToMerchant = false,
        //                                    OrderCode = payRequestToken.RefId,
        //                                    redirectAddress = ""
        //                                };
        //                            }
        //                            else
        //                            {
        //                                result.ErrorCode = 13;
        //                                result.ErrorMessage = "خطا در ثبت سفارش در سمت به پرداخت";
        //                            }
        //                            //}
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

        //public ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string RefId, long ResCode, long SaleOrderId, long SaleReferenceId, string CardHolderInfo, string CardHolderPan, long FinalAmount)
        //{
        //    ServiceResult<RedirectToMerchantModel> result = new ServiceResult<RedirectToMerchantModel>();
        //    try
        //    {
        //        var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.Token == RefId);
        //        if (currentTransaction == null)
        //        {
        //            result.ErrorCode = 12;
        //            result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
        //        }
        //        else
        //        {
        //            if (currentTransaction.Status == 0)
        //            {
        //                var inquaryRequeslt = new BPMVerifyRequestModel
        //                {
        //                    terminalId = long.Parse(currentTransaction.TerminalId),//  long.Parse(merchant.TerminalCode), //6188355,
        //                    userName = "Gholi25",//merchant.UserName
        //                    userPassword = "98143544",//merchant.UserPassword
        //                    orderId = SaleOrderId,
        //                    saleOrderId = SaleOrderId,
        //                    saleRefrenceId = SaleReferenceId
        //                };

        //                var InquiryRequestResultString = _client.bpInquiryRequest(
        //                    inquaryRequeslt.terminalId,
        //                inquaryRequeslt.userName,
        //                    inquaryRequeslt.userPassword,
        //                    inquaryRequeslt.orderId,
        //                    inquaryRequeslt.saleOrderId,
        //                    inquaryRequeslt.saleRefrenceId);

        //                _pSPRequestRepository.Insert(new PspRequest
        //                {
        //                    OrderId = currentTransaction.OrderId,
        //                    PSPId = (long)PSPs.BPM,
        //                    Request = JsonConvert.SerializeObject(inquaryRequeslt),
        //                    Response = JsonConvert.SerializeObject(InquiryRequestResultString),
        //                });

        //                currentTransaction.ReferenceNumber = RefId;
        //                currentTransaction.MID = SaleOrderId.ToString();
        //                currentTransaction.MaskedCarNumber = CardHolderPan;
        //                currentTransaction.RRN = SaleReferenceId.ToString();

        //                switch (int.Parse(InquiryRequestResultString))
        //                {
        //                    case 43:
        //                        currentTransaction.Status = 4; break;
        //                    case 0:
        //                        currentTransaction.Status = 4; break;
        //                    case 44:
        //                        currentTransaction.Status = 2; break;
        //                    case 45:
        //                        currentTransaction.Status = 3; break;
        //                    case 46:
        //                        currentTransaction.Status = 1; break;
        //                    default:
        //                        break;
        //                }


        //                //if (int.Parse(InquiryRequestResultString) == 44)
        //                //{
        //                //    currentTransaction.Status = 2;
        //                //}
        //                //else
        //                //{
        //                //    currentTransaction.Status = 1;
        //                //}

        //                _transactionRepository.Update(currentTransaction);
        //                result.Result = new RedirectToMerchantModel
        //                {
        //                    OrderCode = currentTransaction.Order.OrderCode,
        //                    redirectAddress = currentTransaction.Order.RedirectAddress
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorCode = 99;
        //        result.ErrorMessage = "خطای نا شناخته" + " - ExceptionMessage:" + ex.Message + (ex.InnerException != null ? " - InternalException:" + ex.InnerException.Message : "");
        //        result.Result = null;
        //    }
        //    return result;
        //}

        ////public ServiceResult<bool> CheckTransactionResult(string ResNum)
        ////{
        ////    ServiceResult<bool> result = new ServiceResult<bool>();
        ////    try
        ////    {
        ////        var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.ReserveNumber == ResNum);
        ////        if (currentTransaction == null)
        ////        {
        ////            result.ErrorCode = 12;
        ////            result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
        ////        }
        ////        else
        ////        {
        ////            if (currentTransaction.Status == 0)
        ////            {
        ////                var loginresult = this.Login(currentTransaction.MerchantCode, PSPPassword);
        ////                if (loginresult.ErrorCode == 0)
        ////                {

        ////                    var getinfoparam = new TransReportParam
        ////                    {
        ////                        WSContext = loginresult.Result,
        ////                        reserveNumber = ResNum
        ////                    };
        ////                    var resultinfo = _client.getTransactionReport(getinfoparam);

        ////                    _pSPRequestRepository.Insert(new PSPRequest
        ////                    {
        ////                        OrderId = currentTransaction.OrderId,
        ////                        PSPId = (long)PSPs.PNA,
        ////                        Request = JsonConvert.SerializeObject(getinfoparam),
        ////                        Response = JsonConvert.SerializeObject(resultinfo),
        ////                    });

        ////                    if (resultinfo.result == 0)
        ////                    {
        ////                        if (resultinfo.transactionList != null)
        ////                        {
        ////                            var ctr = resultinfo.transactionList.LastOrDefault();
        ////                            currentTransaction.TransactionRefrenceNumber = ctr.switchRefCode;
        ////                            //currentTransaction.ReferenceNumber = RefNum;
        ////                            //currentTransaction.MID = MID.ToString();
        ////                            currentTransaction.MaskedCarNumber = ctr.maskPan;
        ////                            currentTransaction.RRN = ctr.Rrn;
        ////                            if (ctr.decideReqType == null)
        ////                            {
        ////                                currentTransaction.Status = 2;
        ////                            }
        ////                            else
        ////                            {
        ////                                if (ctr.decideReqType == "R")
        ////                                {
        ////                                    currentTransaction.Status = 3;
        ////                                }
        ////                                else if (ctr.decideReqType == "V")
        ////                                {
        ////                                    currentTransaction.Status = 4;
        ////                                }
        ////                            }
        ////                        }
        ////                        else
        ////                        {
        ////                            currentTransaction.Status = 1;
        ////                        }
        ////                        _transactionRepository.Update(currentTransaction);
        ////                        result.Result = true;
        ////                    }
        ////                    else
        ////                    {
        ////                        result.ErrorCode = 21;
        ////                        result.ErrorMessage = "خطا در دریافت اطلاعات مربوط به تراکنش از پرداخت نوین";
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    result.ErrorCode = loginresult.ErrorCode;
        ////                    result.ErrorMessage = loginresult.ErrorMessage;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        result.ErrorCode = 99;
        ////        result.ErrorMessage = "خطای نا شناخته";
        ////        result.Result = false;
        ////    }
        ////    return result;
        ////}

        //public ServiceResult<bool> VerifyPaymentMethod(Order order)
        //{
        //    ServiceResult<bool> result = new ServiceResult<bool>();

        //    var ltr = order.Transactions.OrderByDescending(t => t.Id).FirstOrDefault();

        //    var verifyRequeslt = new BPMVerifyRequestModel
        //    {
        //        terminalId = long.Parse(ltr.TerminalId),//  long.Parse(merchant.TerminalCode), //6188355,
        //        userName = "Gholi25",//merchant.UserName
        //        userPassword = "98143544",//merchant.UserPassword
        //        orderId = long.Parse(ltr.MID),//== SaleOrderId
        //        saleOrderId = long.Parse(ltr.MID),//== SaleOrderId,
        //        saleRefrenceId = long.Parse(ltr.RRN) //==SaleReferenceId
        //    };

        //    var verifyRequesltResultString = _client.bpVerifyRequest(
        //    verifyRequeslt.terminalId,
        //        verifyRequeslt.userName,
        //        verifyRequeslt.userPassword,
        //        verifyRequeslt.orderId,
        //        verifyRequeslt.saleOrderId,
        //        verifyRequeslt.saleRefrenceId);

        //    _pSPRequestRepository.Insert(new PspRequest
        //    {
        //        OrderId = ltr.OrderId,
        //        PSPId = (long)PSPs.BPM,
        //        Request = JsonConvert.SerializeObject(verifyRequeslt),
        //        Response = JsonConvert.SerializeObject(verifyRequesltResultString),
        //    });

        //    if (int.Parse(verifyRequesltResultString) == 0)
        //    {

        //        var settleRequeslt = new BPMVerifyRequestModel
        //        {
        //            terminalId = long.Parse(ltr.TerminalId),//  long.Parse(merchant.TerminalCode), //6188355,
        //            userName = "Gholi25",//merchant.UserName
        //            userPassword = "98143544",//merchant.UserPassword
        //            orderId = long.Parse(ltr.MID),//== SaleOrderId
        //            saleOrderId = long.Parse(ltr.MID),//== SaleOrderId,
        //            saleRefrenceId = long.Parse(ltr.RRN) //==SaleReferenceId
        //        };

        //        var settleRequesltResultString = _client.bpSettleRequest(
        //        settleRequeslt.terminalId,
        //            settleRequeslt.userName,
        //            settleRequeslt.userPassword,
        //            settleRequeslt.orderId,
        //            settleRequeslt.saleOrderId,
        //            settleRequeslt.saleRefrenceId);

        //        _pSPRequestRepository.Insert(new PspRequest
        //        {
        //            OrderId = ltr.OrderId,
        //            PSPId = (long)PSPs.BPM,
        //            Request = JsonConvert.SerializeObject(settleRequeslt),
        //            Response = JsonConvert.SerializeObject(settleRequesltResultString),
        //        });


        //        switch (int.Parse(settleRequesltResultString))
        //        {
        //            case 43:
        //                ltr.Status = 4; result.Result = true; break;
        //            case 0:
        //                ltr.Status = 4; result.Result = true; break;
        //            //case 44:
        //            //    ltr.Status = 2; break;
        //            //case 45:
        //            //    ltr.Status = 3; break;
        //            //case 46:
        //            //    ltr.Status = 1; break;
        //            default:
        //                result.Result = false;
        //                break;
        //        }
        //        _transactionRepository.Update(ltr);
        //    }
        //    else
        //    {
        //        result.Result = false;
        //    }
        //    //if (verifyResult.Result.Equals("erSucceed"))
        //    //{
        //    //    ltr.Status = 4;
        //    //    result.Result = true;
        //    //    _transactionRepository.Update(ltr);
        //    //}
        //    //else
        //    //{
        //    //    result.Result = false;
        //    //}

        //    return result;
        //}

        ////public ServiceResult<bool> RefoundPaymentMethod(Order order)
        ////{
        ////    ServiceResult<bool> result = new ServiceResult<bool>();

        ////    var ltr = order.Transactions.OrderByDescending(t => t.Id).FirstOrDefault();

        ////    var loginresult = Login(ltr.MerchantCode);
        ////    if (loginresult.ErrorCode == 0)
        ////    {
        ////        var reverseParam = new ReverseMerchantTransParam { WSContext = loginresult.Result, Token = ltr.Token, RefNum = ltr.ReferenceNumber };
        ////        var reverseResult = _client.ReverseMerchantTrans(reverseParam);

        ////        _pSPRequestRepository.Insert(new PSPRequest
        ////        {
        ////            OrderId = ltr.OrderId,
        ////            PSPId = (long)PSPs.PEP,
        ////            Request = JsonConvert.SerializeObject(reverseParam),
        ////            Response = JsonConvert.SerializeObject(reverseResult),
        ////        });

        ////        if (reverseResult.Result.Equals("erSucceed"))
        ////        {
        ////            ltr.Status = 3;
        ////            result.Result = true;
        ////            _transactionRepository.Update(ltr);
        ////        }
        ////        else
        ////        {
        ////            result.Result = false;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        result.ErrorMessage = loginresult.ErrorMessage;
        ////        result.ErrorCode = loginresult.ErrorCode;
        ////    }
        ////    return result;
        ////}
    }
}
