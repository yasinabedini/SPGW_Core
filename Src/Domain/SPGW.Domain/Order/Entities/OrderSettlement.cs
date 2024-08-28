using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Order.Entities
{
    public class OrderSettlement:Entity
    {
        public Nullable<byte> IbanOrderNo { get; set; }

        public Nullable<double> amountPercent { get; set; }

        public long OrderID { get; set; }



        public virtual Order Order { get; set; }
    }
}
