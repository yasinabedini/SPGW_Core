using Newtonsoft.Json;
using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.DTOs;
using SPGW.Domain.Merchant.Entities;
using SPGW.Domain.Order.Entities;
using SPGW.Domain.Psp.Entities;
using SPGW.Domain.Psp.Enums;
using SPGW.Domain.Psp.Repositories;
using SPGW.Domain.Transaction.Entities;
using SPGW.Domain.Transaction.Enums;
using SPGW.Infra.Helpers;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

public class PEPService : IPEPService
{
    public const string RedirectAddress = "https://sepandpay.ir/GW/PEPBack";
    //public const string RedirectAddress = "http://localhost:23000/GW/PEPBack";

    public const string GetTokenAddress = "https://pep.shaparak.ir/pepg/token/getToken";
    public const string CheckTransactionResultAddress = "https://pep.shaparak.ir/Api/v1/Payment/CheckTransactionResult";
    public const string VerifyAddress = "https://pep.shaparak.ir/Api/v1/Payment/VerifyPayment";
    public const string RefoundAddress = "https://pep.shaparak.ir/Api/v1/Payment/RefundPayment";
    public const string RegisterInvoiceAddress = "https://pep.shaparak.ir/pepg/api/payment/purchase";
    public readonly string RSAPrivateKey;
    public readonly string RSAPublicKey;

    private IEFRepository<Customer> _customerRepository;
    private IEFRepository<Order> _orderRepository;
    private IEFRepository<Psp> _pspRepository;
    private IEFRepository<PspRequest> _pSPRequestRepository;
    private IEFRepository<Transaction> _transactionRepository;
    public PEPService(
        IEFRepository<Customer> customerRepository,
        IEFRepository<Order> orderRepository,
        IEFRepository<Psp> pspRepository,
        IEFRepository<PspRequest> pSPRequestRepository,
        IEFRepository<Transaction> transactionRepository
        )
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _pspRepository = pspRepository;
        _pSPRequestRepository = pSPRequestRepository;
        _transactionRepository = transactionRepository;

        var pepPSP = _pspRepository.Find((long)PSPs.PEP);

        ///comment by ali pishkari for demo
        //RSAPrivateKey = pepPSP.RSAPrivateKey;
        //RSAPublicKey = pepPSP.RSAPublicKey;

    }

    public string GetSign(string data)
    {
        //var cs = new CspParameters { KeyContainerName = "PEPContainer" };
        //var rsa = new RSACryptoServiceProvider(cs) { PersistKeyInCsp = false };
        //rsa.Clear();
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(RSAPrivateKey);
        byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA1CryptoServiceProvider());
        string sign = Convert.ToBase64String(signMain);
        return sign;
    }

    public void RegisterInvoice(PEPInvoice invoice, string token)
    {
        var request = (HttpWebRequest)WebRequest.Create(RegisterInvoiceAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional
        var jsonSer = JsonConvert.SerializeObject(invoice);

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var response = (HttpWebResponse)request.GetResponse();
        // DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(PEPResult<RegisterInvoceResult>));

        //var result = (GetTokenResult)ser_resp2.ReadObject(response.GetResponseStream());
    }
    public string CallGetTokenMethod(Merchant merchant)
    {
        var request = (HttpWebRequest)WebRequest.Create(GetTokenAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional
        GetTokenInput input = new GetTokenInput { username = merchant.CmsPreservationId, password = merchant.RsaPublicKey };
        var jsonSer = JsonConvert.SerializeObject(input);

        //request.Headers.Add("Sign", GetSign(jsonSer));

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }

        //uncomment after local test
        //   var response = (HttpWebResponse)request.GetResponse(); 
        //DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(GetTokenResult));

        //var result = (GetTokenResult)ser_resp2.ReadObject(response.GetResponseStream());
        //Console.WriteLine(result.token);//deleteaftertest
        return "123";//deleteaftertest
                     //return result.token;
    }

    public PEPCheckTransactionResult CallCheckTransactionResultMethodByTref(string tref)
    {
        var request = (HttpWebRequest)WebRequest.Create(CheckTransactionResultAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional

        var obj = new
        {
            TransactionReferenceID = tref
        };

        var jsonSer = JsonConvert.SerializeObject(obj);

        //request.Headers.Add("Sign", GetSign(jsonSer));

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var response = (HttpWebResponse)request.GetResponse();
        DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(PEPCheckTransactionResult));
        //var d = ser_resp2.ReadObject(httpResponse2.GetResponseStream());
        var result = (PEPCheckTransactionResult)ser_resp2.ReadObject(response.GetResponseStream());
        return result;
    }

    public PEPCheckTransactionResult CallCheckTransactionResultMethodByiNiD(string iN, string iD, string mc, string tc)

    {
        var request = (HttpWebRequest)WebRequest.Create(CheckTransactionResultAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional

        var obj = new
        {
            InvoiceNumber = iN,
            InvoiceDate = iD,
            TerminalCode = tc,
            MerchantCode = mc
        };

        var jsonSer = JsonConvert.SerializeObject(obj);

        //request.Headers.Add("Sign", GetSign(jsonSer));

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var response = (HttpWebResponse)request.GetResponse();
        DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(PEPCheckTransactionResult));
        //var d = ser_resp2.ReadObject(httpResponse2.GetResponseStream());
        var result = (PEPCheckTransactionResult)ser_resp2.ReadObject(response.GetResponseStream());
        return result;
    }

    public PEPVerifyPaymentResult CallVerifyPaymentMethod(PEPVerifyPaymentParamenterModel invoice)
    {
        var request = (HttpWebRequest)WebRequest.Create(VerifyAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional

        var jsonSer = JsonConvert.SerializeObject(invoice);

        request.Headers.Add("Sign", GetSign(jsonSer));

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var response = (HttpWebResponse)request.GetResponse();
        DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(PEPVerifyPaymentResult));
        //var d = ser_resp2.ReadObject(httpResponse2.GetResponseStream());
        var result = (PEPVerifyPaymentResult)ser_resp2.ReadObject(response.GetResponseStream());
        return result;
    }

    public PEPRefoundPaymentResult CallRefoundPaymentMethod(PEPRefoundPaymentParamenterModel invoice)
    {
        var request = (HttpWebRequest)WebRequest.Create(RefoundAddress);
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 120000;// this line is optional

        var jsonSer = JsonConvert.SerializeObject(invoice);

        request.Headers.Add("Sign", GetSign(jsonSer));

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonSer);
            streamWriter.Flush();
            streamWriter.Close();
        }
        var response = (HttpWebResponse)request.GetResponse();
        DataContractJsonSerializer ser_resp2 = new DataContractJsonSerializer(typeof(PEPRefoundPaymentResult));
        //var d = ser_resp2.ReadObject(httpResponse2.GetResponseStream());
        var result = (PEPRefoundPaymentResult)ser_resp2.ReadObject(response.GetResponseStream());
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
                            if (!psps.Any(p => p.PSPId == (long)PSPs.PEP))
                            {
                                result.ErrorCode = 11;
                                result.ErrorMessage = "مشتری " + currentOrder.Customer.ShopName + " هیچ درگاه فعال پاسارگاد ندارد. لطفا با پشتیبانی تماس بگیرید";
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
                                var merchant = currentOrder.Customer.Merchants.FirstOrDefault(m => m.PSPId == (long)PSPs.PEP);
                                var now = DateTime.Now;
                                var invoice = new PEPInvoice
                                {
                                    amount = (int)currentOrder.Amount,
                                    callbackApi = RedirectAddress,
                                    description = "",
                                    invoice = currentOrder.InvoiceNumber.ToString(),
                                    invoiceDate = currentOrder.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss"),
                                    serviceCode = 8,
                                    serviceType = "PURCHASE",
                                    terminalNumber = int.Parse(merchant.TerminalCode),





                                };
                                var gettokenResult = CallGetTokenMethod(merchant);

                                _pSPRequestRepository.Insert(new PspRequest
                                {
                                    OrderId = currentOrder.Id,
                                    PSPId = merchant.PSPId,
                                    Request = JsonConvert.SerializeObject(invoice),
                                    Response = JsonConvert.SerializeObject(gettokenResult)
                                });

                                if (currentTransaction != null)
                                {
                                    currentTransaction.PaymentDate = now;
                                    currentTransaction.PSPId = merchant.PSPId;
                                    currentTransaction.InvoiceDateString = invoice.invoiceDate;
                                    currentTransaction.InvoiceNumber = invoice.invoice;

                                    currentTransaction.PayerName = payerName;
                                    currentTransaction.PayerMobileNumber = payerMobile;

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
                                        PaymentTime = now.TimeOfDay,//  int.Parse(now.ToPersianDateTimeString_Time()),

                                        PSPId = merchant.PSPId,
                                        Status = 0,
                                        RRN = "",

                                        InvoiceDateString = invoice.invoiceDate,
                                        InvoiceNumber = invoice.invoice,
                                        TransactionRefrenceNumber = "",

                                        PayerName = payerName,
                                        PayerMobileNumber = payerMobile
                                    });
                                }

                                if (gettokenResult != "")
                                {
                                    result.Result = new PSPDetailResult
                                    {
                                        RedirectToMerchant = false,
                                        OrderCode = gettokenResult,
                                        redirectAddress = ""
                                    };
                                    //result.Result = gettokenResult.Token;
                                }
                                else
                                {
                                    result.ErrorCode = 20;
                                    result.ErrorMessage = "Error in getting token(PEP)";//gettokenResult.Message;
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

    public ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string tref, string iN, string iD)
    {
        ServiceResult<RedirectToMerchantModel> result = new ServiceResult<RedirectToMerchantModel>();
        try
        {
            var currentTransaction = _transactionRepository.Queryable().FirstOrDefault(t => t.InvoiceDateString == iD && t.InvoiceNumber == iN);
            if (currentTransaction == null)
            {
                result.ErrorCode = 12;
                result.ErrorMessage = "فاکتور مورد نظر وجود ندارد";
            }
            else
            {
                if (currentTransaction.Status == 0)
                {
                    var checkResult = CallCheckTransactionResultMethodByTref(tref);

                    _pSPRequestRepository.Insert(new PspRequest
                    {
                        OrderId = currentTransaction.OrderId,
                        PSPId = (long)PSPs.PEP,
                        Request = JsonConvert.SerializeObject(new { TransactionReferenceID = tref }),
                        Response = JsonConvert.SerializeObject(checkResult),
                    });

                    if (!checkResult.IsSuccess)
                    {
                        //Should be changed later
                        //result.ErrorCode = 13;
                        //result.ErrorMessage = checkResult.Message;
                        currentTransaction.Status = 1;
                    }
                    else
                    {
                        currentTransaction.TransactionRefrenceNumber = checkResult.TransactionReferenceID;
                        currentTransaction.ReferenceNumber = checkResult.ReferenceNumber.ToString();
                        currentTransaction.Status = 2;
                        //result.Result = currentTransaction.Order.OrderCode;
                    }
                    _transactionRepository.Update(currentTransaction);

                }
                result.Result = new RedirectToMerchantModel
                {
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
}