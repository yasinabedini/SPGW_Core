using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.Psp.Entities;

namespace SPGW.Domain.Order.Entities
{
    public class Order:Entity
    {
 
        
        public long CustomerId { get; set; }

        public long TotalAmount { get; set; }

        public long Amount { get; set; }

        public string RedirectAddress { get; set; }

        public long InvoiceNumber { get; set; }

        public Nullable<System.DateTime> InvoiceDate { get; set; }

        public System.DateTime RegisterDate { get; set; }

        public string OrderCode { get; set; }

        public Nullable<long> TodayOrderNumber { get; set; }

        public bool DirectPSPGW { get; set; }

        public Nullable<int> PSPCode { get; set; }



        public virtual Customer.Entities.Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<OrderSettlement> OrderSettlements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<PspRequest> PSPRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Transaction.Entities.Transaction> Transactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderSettlements = new HashSet<OrderSettlement>();

            this.PSPRequests = new HashSet<PspRequest>( );

            this.Transactions = new HashSet<Transaction.Entities.Transaction>();
        }

    }
}
