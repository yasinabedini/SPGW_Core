using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Customer.Entities
{
    public class CustomerIban:Entity
    {
        public string Iban { get; set; }
        public long CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public byte ShareType { get; set; }
        public float SharedAmount { get; set; }
        public long ShareAmountMax { get; set; }
        public long ShareAmountMin { get; set; }
        public bool IsMain { get; set; }
    }
}
