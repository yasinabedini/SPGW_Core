using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.DTOs
{
    //public class PEPInvoice
    //{
    //    public string InvoiceNumber { get; set; }
    //    public string InvoiceDate { get; set; }
    //    public string TerminalCode { get; set; }
    //    public string MerchantCode { get; set; }
    //    public decimal Amount { get; set; }
    //    public string RedirectAddress { get; set; }
    //    public string Timestamp { get; set; }
    //    public int Action { get; set; }
    //}

    //public class PEPGetTokenResult
    //{
    //    public bool IsSuccess { get; set; }
    //    public string Message { get; set; }
    //    public string Token { get; set; }
    //}

    public class PEPCheckTransactionResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int TraceNumber { get; set; }
        public long ReferenceNumber { get; set; }
        public string TransactionDate { get; set; }
        public string Action { get; set; }
        public string TransactionReferenceID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string MerchantCode { get; set; }
        public string TerminalCode { get; set; }
        public string Amount { get; set; }
    }

    public class RedirectToMerchantModel
    {
        public string redirectAddress { get; set; }
        public string OrderCode { get; set; }
        public bool DirectPSPGW { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public class PSPDetailResult
    {
        public bool RedirectToMerchant { get; set; }
        public string redirectAddress { get; set; }
        public string OrderCode { get; set; }
    }

    public class PEPVerifyPaymentParamenterModel
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string TerminalCode { get; set; }
        public string MerchantCode { get; set; }
        public decimal Amount { get; set; }
        public string TimeStamp { get; set; }
    }

    public class PEPVerifyPaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string MaskedCardNumber { get; set; }
        public string HashedCardNumber { get; set; }
        public string ShaparakRefNumber { get; set; }
    }

    public class PEPRefoundPaymentParamenterModel
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string TerminalCode { get; set; }
        public string MerchantCode { get; set; }
        public string TimeStamp { get; set; }
    }

    public class PEPRefoundPaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class GetTokenInput
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class PEPInvoice
    {
        public int amount { get; set; }
        public string callbackApi { get; set; }
        public string description { get; set; }
        public string invoice { get; set; }
        public string invoiceDate { get; set; }
        public string mobileNumber { get; set; }
        public string payerMail { get; set; }
        public string payerName { get; set; }
        public int serviceCode { get; set; }
        public string serviceType { get; set; }
        public int terminalNumber { get; set; }
        public string nationalCode { get; set; }
    }
}