using Microsoft.AspNetCore.Http;
using SPGW.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Common
{
    public class PerHttpEFContextUnitOfWorkScope : EfUnitOfWorkScope
    {
        private readonly string key = Guid.NewGuid().ToString();
        private readonly HttpContext _httpContext;

        public PerHttpEFContextUnitOfWorkScope(IEFUnitOfWorkFactory unitOfWorkFactory, IHttpContextAccessor httpContextAccessor)
          : base(unitOfWorkFactory)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        protected override IEFUnitOfWork LoadUnitOfWork()
        {
            try
            {
                return (IEFUnitOfWork)_httpContext.Items[key];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void SaveUnitOfWork(IEFUnitOfWork unitOfWork)
        {
            try
            {                
                _httpContext.Items[key] = unitOfWork; ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
