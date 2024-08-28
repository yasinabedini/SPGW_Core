using SPGW.Domain.Psp.Entities;
using SPGW.Domain.Psp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.DTOs
{
    public class PayOrderModel
    {
        public long Amount { get; set; }
        public long TotalAmount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public long InvoiceNumber { get; set; }
        public string RedirectAddress { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public PSPs[] GateWays { get; set; }

        public string OrderCode { get; set; }

        public string Logo { get; set; }

        public bool DirectPSPGW { get; set; }
        public PSPs? PSP { get; set; }

    }
}
