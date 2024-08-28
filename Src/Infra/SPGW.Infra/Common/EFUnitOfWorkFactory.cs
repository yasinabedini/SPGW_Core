using SPGW.Domain.Common;
using SPGW.Infra.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Common
{
    public class FRMSEFUnitOfWorkFactory : IEFUnitOfWorkFactory
    {
        private readonly Func<SPGWContext> dbDelegate;

        public FRMSEFUnitOfWorkFactory(Func<SPGWContext> dbDelegate)
        {
            this.dbDelegate = dbDelegate;
        }

        public IEFUnitOfWork Creat()
        {
            return (IEFUnitOfWork)new EFUnitOfWork(this.dbDelegate());
        }
    }
}
