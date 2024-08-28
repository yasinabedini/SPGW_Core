using SPGW.Domain.Common;
using SPGW.Infra.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Common
{
    public class EFUnitOfWork : IEFUnitOfWork, IDisposable
    {
        public SPGWContext CPH4Context { get; protected set; }

        public bool LazyLoadingEnabled
        {
            get => CPH4Context.LazyLoadingEnabled;
            set => CPH4Context.LazyLoadingEnabled = value;
        }

        public bool ProxyCreationEnabled
        {
            get => CPH4Context.ProxyCreationEnabled;
            set => CPH4Context.ProxyCreationEnabled = value;
        }

        public EFUnitOfWork(SPGWContext context)
        {
            CPH4Context = context;
        }

        public void Commit()
        {
            CPH4Context.SaveChanges();
        }

        public void Dispose()
        {
            CPH4Context.Dispose();
        }
    }
}
