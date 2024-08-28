using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPGW.Domain.Psp.Entities
{
    public class Psp:Entity
    {  
        public string Title { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RSAPrivateKey { get; set; }

        public string RSAPublicKey { get; set; }

        public string PaymentAddress { get; set; }

        public string RedirectAddress { get; set; }

        public bool IsActive { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Merchant.Entities.Merchant> Merchants { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<PspRequest> PSPRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Transaction.Entities.Transaction> Transactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Psp()
        {

            this.Merchants = new HashSet<Merchant.Entities.Merchant>();

            this.PSPRequests = new HashSet<PspRequest>();

            this.Transactions = new HashSet<Transaction.Entities.Transaction>();
        }
    }
}
