using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Common
{
    public class EfUnitOfWorkScope : IEFUnitOfWorkScope
    {
        private readonly IEFUnitOfWorkFactory _unitOfWorkFactory;
        private IEFUnitOfWork _unitOfWork;

        public IEFUnitOfWork CurrentUnitOfWork
        {
            get
            {
                IEFUnitOfWork unitOfWork = LoadUnitOfWork();
                if (unitOfWork == null)
                {
                    unitOfWork = _unitOfWorkFactory.Creat();
                    SaveUnitOfWork(unitOfWork);
                }
                return unitOfWork;
            }
        }

        public bool LazyLoadingEnabled
        {
            get
            {
                return CurrentUnitOfWork.LazyLoadingEnabled;
            }
            set
            {
                CurrentUnitOfWork.LazyLoadingEnabled = value;
            }
        }

        public bool ProxyCreationEnabled
        {
            get
            {
                return CurrentUnitOfWork.ProxyCreationEnabled;
            }
            set
            {
                CurrentUnitOfWork.ProxyCreationEnabled = value;
            }
        }

        public EfUnitOfWorkScope(IEFUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Commit()
        {
            CurrentUnitOfWork.Commit();
        }

        protected virtual IEFUnitOfWork LoadUnitOfWork()
        {
            return _unitOfWork;
        }

        protected virtual void SaveUnitOfWork(IEFUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
