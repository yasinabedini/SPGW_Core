using Microsoft.EntityFrameworkCore;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.Merchant.Entities;
using SPGW.Domain.Order.Entities;
using SPGW.Domain.Psp.Entities;
using SPGW.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Contexts
{
    public class SPGWContext : DbContext
    {
        public SPGWContext(DbContextOptions<SPGWContext> options)
    : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerIban> CustomerIbans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderSettlement> OrderSettlements { get; set; }
        public DbSet<PspRequest> PspRequests { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Psp> Psps { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        public bool LazyLoadingEnabled
        {
            get => this.ChangeTracker.LazyLoadingEnabled;
            set => this.ChangeTracker.LazyLoadingEnabled = value;
        }

        public bool ProxyCreationEnabled
        {
            get => this.ChangeTracker.AutoDetectChangesEnabled;
            set => this.ChangeTracker.AutoDetectChangesEnabled = value;
        }

        public static Func<SPGWContext> UnitOfWorkContextConstructor
        {
            get
            {
                return () =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<SPGWContext>();
                    optionsBuilder.UseSqlServer("YourConnectionStringHere");

                    var db = new SPGWContext(optionsBuilder.Options);

                    db.ChangeTracker.LazyLoadingEnabled = true;
                    db.ChangeTracker.AutoDetectChangesEnabled = true;
                    return db;
                };
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
          .HasOne(c => c.Order)
          .WithMany()
          .HasForeignKey(t=>t.OrderId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(s => s.Transactions)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
