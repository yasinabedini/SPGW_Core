using SPGW.Domain.Common;
using SPGW.Domain.Psp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Transaction.Entities
{
    public class Transaction:Entity
    {
        public long CustomerId { get; set; }

        public long PSPId { get; set; }

        public string MerchantCode { get; set; }

        public long OrderId { get; set; }

        public long TotalAmount { get; set; }

        public long Amount { get; set; }

        public string TransactionRefrenceNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public string InvoiceDateString { get; set; }

        public string ReferenceNumber { get; set; }

        public string RRN { get; set; }

        public string MaskedCarNumber { get; set; }

        public Nullable<int> PPaymentDate { get; set; }

        public Nullable<System.DateTime> PaymentDate { get; set; }

        public Nullable<System.TimeSpan> PaymentTime { get; set; }

        public string PayerName { get; set; }

        public string PayerMobileNumber { get; set; }

        public string ReserveNumber { get; set; }

        public string Token { get; set; }

        public string MID { get; set; }

        public string SystemTraceAuditNumber { get; set; }

        public Nullable<int> Status { get; set; }

        public string TerminalId { get; set; }

        public string ResponseCode { get; set; }



        public virtual Customer.Entities.Customer Customer { get; set; }

        public virtual Order.Entities.Order Order { get; set; }

        public virtual Psp.Entities.Psp PSP { get; set; }
    }
}
