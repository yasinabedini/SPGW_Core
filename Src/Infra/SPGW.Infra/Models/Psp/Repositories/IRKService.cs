using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using SPGW.Domain.Common;
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

namespace SPGW.Infra.Models.Psp.Repositories
{
    public class IRKService : IIRKService
    {
        private string RevertUrl;
        private const string TokenUrl = "https://ikc.shaparak.ir/api/v3/tokenization/make";
        private const string VerifyUrl = "https://ikc.shaparak.ir/api/v3/confirmation/purchase";
        private const string InqueryUrl = "https://ikc.shaparak.ir/api/v3/inquiry/single";
        private const string RefoundUrl = "https://ikc.shaparak.ir/api/v3/confirmation/reversePurchase";

        private IEFRepository<Domain.Order.Entities.Order> _orderRepository;
        private IEFRepository<PspRequest> _pSPRequestRepository;
        private IEFRepository<Transaction> _transactionRepository;
        public IRKService(
            IEFRepository<Domain.Order.Entities.Order> orderRepository,
            IEFRepository<PspRequest> pSPRequestRepository,
            IEFRepository<Transaction> transactionRepository,
            IOptions<InfraSetting> setting
            )
        {
            _orderRepository = orderRepository;
            _pSPRequestRepository = pSPRequestRepository;
            _transactionRepository = transactionRepository;
            RevertUrl = setting.Value.IRKGateWayRedirectAddress;
        }

        public IRKRefoundPaymentResult CallRefoundPaymentMethod(IRKRefoundPaymentParamenterModel invoice)
        {
            var result = new IRKRefoundPaymentResult();
            try
            {
                if (!string.IsNullOrEmpty(invoice.ResponseCode) && invoice.ResponseCode == "00")
                {
                    WebHelper webHelper = new WebHelper();

                    RequestVerify requestVerify = new RequestVerify();
                    requestVerify.terminalId = invoice.TerminalCode;
                    requestVerify.systemTraceAuditNumber = invoice.TraceNo;
                    requestVerify.retrievalReferenceNumber = string.Empty;
                    requestVerify.tokenIdentity = invoice.Token;

                    string request = JsonConvert.SerializeObject(requestVerify);

                    Uri url = new Uri(RefoundUrl);
                    string jresponse = webHelper.Post(url, request);
                    var inqResult = Inquery(invoice.Token, invoice.TerminalCode, invoice.PassPhrase);

                    if (jresponse != null)
                    {
                        VerifyResult jResult = JsonConvert.DeserializeObject<VerifyResult>(jresponse);

                        if (jResult.status)
                        {
                            var message = "عملیات برگشت تراکنش با موفقیت انجام شد" + " result=" + jResult.description;
                            result = new IRKRefoundPaymentResult { IsSuccess = true, Message = message };
                        }
                        else
                        {
                            var message = "عملیات برگشت تراکنش با موفقیت انجام نشد" + " result=" + jResult.description;
                            result = new IRKRefoundPaymentResult { IsSuccess = false, Message = message };
                        }
                    }
                }
                else
                {
                    var message = "تراکنش  نا موفق بوده است";
                    result = new IRKRefoundPaymentResult { IsSuccess = false, Message = message };
                }
            }
            catch (Exception exe)
            {
                var message = "خطا" + exe.Message;
                result = new IRKRefoundPaymentResult { IsSuccess = false, Message = message };
            }

            return result;
        }

        private InquiryResult Inquery(string token, string terminalId, string passPhrase)
        {
            try
            {
                WebHelper webHelper = new WebHelper();
                Inquery inquery = new Inquery();
                inquery.terminalId = terminalId;
                inquery.requestId = string.Empty;
                inquery.retrievalReferenceNumber = string.Empty;
                inquery.passPhrase = passPhrase;
                inquery.tokenIdentity = token;
                inquery.findOption = 2;
                string request = JsonConvert.SerializeObject(inquery);

                Uri url = new Uri(InqueryUrl);
                var jresponse = webHelper.Post(url, request);

                if (jresponse != null)
                {
                    InquiryResult jResult = JsonConvert.DeserializeObject<InquiryResult>(jresponse);
                    return jResult;
                }

                return new InquiryResult
                {
                    Status = false,
                    Result = "عملیات با موفقیت انجام نشد."
                };
            }
            catch (Exception ex)
            {
                return new InquiryResult
                {
                    Status = false,
                    Result = "عملیات با موفقیت انجام نشد.",
                    Description = ex.Message
                };
            }
        }

        public IRKVerifyPaymentResult CallVerifyPaymentMethod(IRKVerifyPaymentParamenterModel invoice)
        {
            var result = new IRKVerifyPaymentResult();
            try
            {
                if (!string.IsNullOrEmpty(invoice.ResponseCode) && invoice.ResponseCode == "00")
                {
                    WebHelper webHelper = new WebHelper();

                    RequestVerify requestVerify = new RequestVerify();
                    requestVerify.terminalId = invoice.TerminalCode;
                    requestVerify.systemTraceAuditNumber = invoice.TraceNo;
                    requestVerify.retrievalReferenceNumber = invoice.RRN;
                    requestVerify.tokenIdentity = invoice.Token;

                    string request = JsonConvert.SerializeObject(requestVerify);

                    Uri url = new Uri(VerifyUrl);
                    string jresponse = webHelper.Post(url, request);

                    if (jresponse != null)
                    {
                        VerifyResult jResult = JsonConvert.DeserializeObject<VerifyResult>(jresponse);

                        if (jResult.status)
                        {
                            var message = "عملیات تایید تراکنش با موفقیت انجام شد" + " result=" + jResult.description;
                            result = new IRKVerifyPaymentResult { IsSuccess = true, Message = message };
                        }
                        else
                        {
                            var message = "عملیات تایید تراکنش با موفقیت انجام نشد" + " result=" + jresponse;//jResult.description;
                            result = new IRKVerifyPaymentResult { IsSuccess = false, Message = message };
                        }
                    }
                }
                else
                {
                    var message = "تراکنش  نا موفق بوده است";
                    result = new IRKVerifyPaymentResult { IsSuccess = false, Message = message };
                }
            }
            catch (Exception exe)
            {
                var message = "خطا" + exe.Message;
                result = new IRKVerifyPaymentResult { IsSuccess = false, Message = message };
            }

            return result;
        }

        public ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string token, string responseCode, string RequestId, string paymentId, string retrievalReferenceNumber, string systemTraceAuditNumber)
        {
            ServiceResult<RedirectToMerchantModel> result = new ServiceResult<RedirectToMerchantModel>();
            try
            {
                var currentTransaction = _transactionRepository.Queryable().OrderByDescending(t => t.Id).FirstOrDefault(t => t.InvoiceNumber.ToString() == RequestId);
                if (currentTransaction == null)
                {
                    result.ErrorCode = 12;
                    result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
                }
                else
                {
                    var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => o.Id.ToString() == paymentId);
                    var merchant = currentOrder.Customer.Merchants.FirstOrDefault(t => t.TerminalCode == currentTransaction.TerminalId);
                    var isSuccess = false;

                    if (currentTransaction.Status == 0)
                    {
                        var checkResult = Inquery(token, merchant.TerminalCode, merchant.PassPhrase);

                        _pSPRequestRepository.Insert(new PspRequest
                        {
                            OrderId = currentTransaction.OrderId,
                            PSPId = (long)PSPs.IRK,
                            Request = JsonConvert.SerializeObject(new { Token = token, TerminalCode = merchant.TerminalCode, PassPhrase = merchant.PassPhrase }),
                            Response = JsonConvert.SerializeObject(checkResult),
                        });

                        if (!checkResult.Status)
                        {
                            currentTransaction.Status = 1;
                        }
                        else
                        {
                            isSuccess = true;
                            currentTransaction.Status = 2;
                        }

                        currentTransaction.ResponseCode = responseCode;
                        currentTransaction.RRN = retrievalReferenceNumber;
                        currentTransaction.SystemTraceAuditNumber = systemTraceAuditNumber;

                        _transactionRepository.Update(currentTransaction);
                    }
                    result.Result = new RedirectToMerchantModel
                    {
                        IsSuccessful = isSuccess,
                        OrderCode = currentTransaction.Order.OrderCode,
                        redirectAddress = currentTransaction.Order.RedirectAddress
                    };
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = "خطای نا شناخته";
                result.Result = null;
            }
            return result;
        }

        public ServiceResult<object> GetPSPDetails(string orderCode, string payerName, string payerMobile)
        {
            var result = new ServiceResult<object>();
            try
            {
                if (string.IsNullOrEmpty(orderCode))
                {
                    result.ErrorCode = 7;
                    result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
                }
                else
                {
                    var currentOrder = _orderRepository.Queryable().FirstOrDefault(o => o.OrderCode == orderCode);
                    if (currentOrder == null)
                    {
                        result.ErrorCode = 7;
                        result.ErrorMessage = "کد سفارش ارسال شده نا معتبر است";
                    }
                    else
                    {
                        if (!currentOrder.Customer.IsActive)
                        {
                            result.ErrorCode = 8;
                            result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " غیر فعال است. لطفا با پشتیبانی تماس بگیرید";
                        }
                        else
                        {
                            var merchants = currentOrder.Customer.Merchants.Where(m => m.IsActive);
                            if (!merchants.Any())
                            {
                                result.ErrorCode = 9;
                                result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ پذیرنده فعالی ندارد. لطفا با پشتیبانی تماس بگیرید";
                            }
                            else
                            {
                                var psps = merchants.Where(m => m.PSP.IsActive);
                                if (!psps.Any(p => p.PSPId == (long)PSPs.IRK))
                                {
                                    result.ErrorCode = 11;
                                    result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ درگاه فعال ایران کیش ندارد. لطفا با پشتیبانی تماس بگیرید";
                                }
                                else
                                {
                                    var currentTransaction = _transactionRepository.Queryable().OrderByDescending(t => t.Id).FirstOrDefault(t => t.OrderId == currentOrder.Id);
                                    if (currentTransaction != null && currentTransaction.Status != (int)TransactionStates.Register)
                                    {
                                        result.Result = new PSPDetailResult
                                        {
                                            RedirectToMerchant = true,
                                            OrderCode = currentOrder.OrderCode,
                                            redirectAddress = currentOrder.RedirectAddress
                                        };
                                    }
                                    var merchant = currentOrder.Customer.Merchants.FirstOrDefault(m => m.PSPId == (long)PSPs.IRK);
                                    var now = DateTime.Now;
                                    var invoice = new IRKInvoice
                                    {
                                        Amount = currentOrder.Amount,
                                        AcceptorId = merchant.AcceptorId,
                                        CmsPreservationId = merchant.CmsPreservationId,
                                        PassPhrase = merchant.PassPhrase,
                                        TerminalId = merchant.TerminalCode,
                                        RsaPublicKey = merchant.RsaPublicKey,
                                        PaymentId = currentOrder.Id.ToString(),
                                        RequestId = currentOrder.InvoiceNumber.ToString()//currentTransaction.Id.ToString()
                                    };
                                    var gettokenResult = CallGetTokenMethod(invoice);

                                    _pSPRequestRepository.Insert(new PspRequest
                                    {
                                        OrderId = currentOrder.Id,
                                        PSPId = merchant.PSPId,
                                        Request = JsonConvert.SerializeObject(invoice),
                                        Response = JsonConvert.SerializeObject(gettokenResult),
                                    });
                                    if (gettokenResult.IsSuccess)
                                    {
                                        if (currentTransaction != null)
                                        {
                                            currentTransaction.PaymentDate = now;
                                            currentTransaction.PSPId = merchant.PSPId;
                                            currentTransaction.InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss");
                                            currentTransaction.InvoiceNumber = currentOrder.InvoiceNumber.ToString();

                                            currentTransaction.PayerName = payerName;
                                            currentTransaction.PayerMobileNumber = payerMobile;

                                            currentTransaction.TerminalId = merchant.TerminalCode;

                                            currentTransaction.Token = gettokenResult.Token;

                                            _transactionRepository.Update(currentTransaction);
                                        }
                                        else
                                        {
                                            _transactionRepository.Insert(new Transaction
                                            {
                                                Amount = currentOrder.Amount,
                                                TotalAmount = currentOrder.TotalAmount,
                                                CustomerId = currentOrder.CustomerId,
                                                MerchantCode = merchant.MerchantCode,
                                                OrderId = currentOrder.Id,

                                                PaymentDate = now,
                                                PPaymentDate = int.Parse(now.ToPersianDateTimeString_PPAymentDate()),
                                                PaymentTime = now.TimeOfDay,

                                                TerminalId = merchant.TerminalCode,

                                                PSPId = merchant.PSPId,
                                                Status = 0,
                                                RRN = "",

                                                InvoiceDateString = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss"),
                                                InvoiceNumber = currentOrder.InvoiceNumber.ToString(),
                                                TransactionRefrenceNumber = "",
                                                Token = gettokenResult.Token,

                                                PayerName = payerName,
                                                PayerMobileNumber = payerMobile
                                            });
                                        }


                                        result.Result = new PSPDetailResult
                                        {
                                            RedirectToMerchant = false,
                                            OrderCode = gettokenResult.Token,
                                            redirectAddress = ""
                                        };
                                    }
                                    else
                                    {
                                        result.ErrorCode = 20;
                                        result.ErrorMessage = gettokenResult.Message;// +JsonConvert.SerializeObject(gettokenResult);

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }

        public IRKGetTokenResult CallGetTokenMethod(IRKInvoice invoice)
        {
            var result = new IRKGetTokenResult();
            try
            {
                WebHelper webHelper = new WebHelper();

                string paymentId = invoice.PaymentId;
                string requestId = invoice.RequestId;
                string request = string.Empty;
                IPGData iPGData = new IPGData();
                iPGData.TreminalId = invoice.TerminalId;
                iPGData.AcceptorId = invoice.AcceptorId;

                iPGData.RevertURL = RevertUrl;
                iPGData.Amount = invoice.Amount;
                iPGData.PaymentId = paymentId;
                iPGData.RequestId = requestId;
                iPGData.CmsPreservationId = invoice.CmsPreservationId;
                iPGData.TransactionType = "Purchase";
                iPGData.BillInfo = null;
                iPGData.PassPhrase = invoice.PassPhrase;
                iPGData.RsaPublicKey = invoice.RsaPublicKey;

                request = CreateJsonRequest.CreateJasonRequest(iPGData);

                Uri url = new Uri(TokenUrl);
                string jresponse = webHelper.Post(url, request);

                if (jresponse != null)
                {
                    TokenResult jResult = JsonConvert.DeserializeObject<TokenResult>(jresponse);

                    if (jResult.status)
                    {
                        var token = jResult.result?.token;
                        result = new IRKGetTokenResult
                        {
                            IsSuccess = true,
                            Message = "توکن با موفقیت دریافت شد.",
                            Token = token
                        };
                    }
                    else
                    {
                        var message = string.Format("result:{0} desc:{1}", jResult.responseCode, jResult.description);
                        result = new IRKGetTokenResult { IsSuccess = false, Message = message };
                    }
                }
                result.Message += " - " + jresponse;
            }
            catch (Exception exe)
            {

                result = new IRKGetTokenResult { IsSuccess = false, Message = exe.Message };
            }

            return result;
        }
    }
}
