using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Psp.Entities
{
    public class PspRequest:Entity
    {        
        public long OrderId { get; set; }

        public Nullable<long> PSPId { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }



        public virtual Order.Entities.Order Order { get; set; }

        public virtual Psp PSP { get; set; }
    }
}
