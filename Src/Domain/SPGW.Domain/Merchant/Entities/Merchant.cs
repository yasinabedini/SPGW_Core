using SPGW.Domain.Common;
using SPGW.Domain.Psp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Merchant.Entities
{
   public class Merchant:Entity
    {        
        public long CustomerId { get; set; }

        public long PSPId { get; set; }

        public string MerchantCode { get; set; }

        public string TerminalCode { get; set; }

        public bool IsActive { get; set; }

        public int uid { get; set; }

        public string PassPhrase { get; set; }

        public string TraceNo { get; set; }

        public string AcceptorId { get; set; }

        public string CmsPreservationId { get; set; }

        public string RsaPublicKey { get; set; }



        public virtual Customer.Entities.Customer Customer { get; set; }

        public virtual Psp.Entities.Psp PSP { get; set; }
    }
}
