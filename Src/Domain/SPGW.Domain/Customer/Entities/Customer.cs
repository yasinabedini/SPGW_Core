using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPGW.Domain.Customer.Entities
{
    public class Customer : Entity
    {
        public string CustomerCode { get; set; }

        public string Password { get; set; }

        public string ShopName { get; set; }

        public string Logo { get; set; }

        public string WebSiteAddress { get; set; }

        public bool IsActive { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Merchant.Entities.Merchant> Merchants { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Order.Entities.Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Transaction.Entities.Transaction> Transactions { get; set; }


        public Customer()
        {

            this.Merchants = new HashSet<Merchant.Entities.Merchant>();

            this.Orders = new HashSet<Order.Entities.Order>();

            this.Transactions = new HashSet<Transaction.Entities.Transaction>();

        }
    }
}
