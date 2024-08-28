using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.DTOs
{
    public class OrderModel
    {
        public OrderModel()
        {
            DirectPSPGW = false;
            PSPCode = 0;
        }
        public long Id { get; set; }
        public string CustomerCode { get; set; }
        public long TotalAmount { get; set; }
        public long Amount { get; set; }
        public string RedirectAddress { get; set; }
        public long InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string OrderCode { get; set; }

        public bool DirectPSPGW { get; set; }
        public int PSPCode { get; set; }
    }

    public class OrderDetails
    {
        public long Id { get; set; }
        public string CustomerCode { get; set; }
        public long TotalAmount { get; set; }
        public long Amount { get; set; }
        public string RedirectAddress { get; set; }
        public long InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string OrderCode { get; set; }

        public bool DirectPSPGW { get; set; }
        public int? PSPCode { get; set; }

        public string PaymentDate { get; set; }
        public string TransactionRefrenceNumber { get; set; }
        public string MaskedPan { get; set; }
        public string RRN { get; set; }
        public int? Status { get; set; }

    }
}
