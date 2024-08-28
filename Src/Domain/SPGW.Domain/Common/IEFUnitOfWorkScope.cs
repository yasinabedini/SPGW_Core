using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Common
{
    public interface IEFUnitOfWorkScope
    {
        IEFUnitOfWork CurrentUnitOfWork { get; }

        bool LazyLoadingEnabled { get; set; }

        bool ProxyCreationEnabled { get; set; }

        void Commit();
    }
}
