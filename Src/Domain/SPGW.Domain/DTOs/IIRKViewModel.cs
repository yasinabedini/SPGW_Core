using System;

namespace SPGW.Domain.DTOs
{
    public class IRKInvoice
    {
        public long Amount { get; set; }
        public string PaymentId { get; set; }
        public string RequestId { get; set; }
        public string TerminalId { get; set; }
        public string AcceptorId { get; set; }
        public string CmsPreservationId { get; set; }
        public string PassPhrase { get; set; }
        public string RsaPublicKey { get; set; }
    }

    public class IRKCheckTransactionResult
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

    public class IRKVerifyPaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class IRKVerifyPaymentParamenterModel
    {
        public string TerminalCode { get; set; }
        public string RRN { get; set; }
        public string Token { get; set; }
        public string ResponseCode { get; set; }
        public string TraceNo { get; set; }
    }

    public class IRKRefoundPaymentParamenterModel
    {
        public string TerminalCode { get; set; }
        public string TraceNo { get; set; }
        public string Token { get; set; }
        public string ResponseCode { get; set; }
        public string PassPhrase { get; set; }
    }

    public class IRKRefoundPaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class IRKGetTokenResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }

    public class VerifyResult
    {
        public string responseCode { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public SubResult result { get; set; }
    }

    public class RequestVerify
    {
        public string terminalId { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string tokenIdentity { get; set; }
    }

    public class SubResult
    {
        public string responseCode { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public DateTime transactionDateTime { get; set; }
        public int transactionDate { get; set; }
        public int transactionTime { get; set; }
        public string processCode { get; set; }
        public object requestId { get; set; }
        public object additional { get; set; }
        public object billType { get; set; }
        public object billId { get; set; }
        public string paymentId { get; set; }
        public string amount { get; set; }
        public object revertUri { get; set; }
        public object acceptorId { get; set; }
        public object terminalId { get; set; }
        public object tokenIdentity { get; set; }
    }

    public class Inquery
    {
        public int findOption { get; set; }
        public string passPhrase { get; set; }
        public string requestId { get; set; }
        public object retrievalReferenceNumber { get; set; }
        public string terminalId { get; set; }
        public object tokenIdentity { get; set; }
    }

    public class InquiryResult
    {
        public bool Status { get; set; }
        public object Result { get; set; }
        public string Description { get; set; }
        public string ResponseCode { get; set; }
    }
}