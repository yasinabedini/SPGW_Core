using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;
using SPGW.Domain.Customer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Models.Customer.Repositories
{
    public class UserManagerService : IUserManagerService
    {
        private IEFRepository<Domain.Customer.Entities.Customer> _customerRepository;
        public UserManagerService(IEFRepository<Domain.Customer.Entities.Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public ServiceResult<bool> HasAccess(string username, string password)
        {
            var currentuser = _customerRepository.Queryable().FirstOrDefault(c => c.Password == password && c.CustomerCode == username);
            if (currentuser != null)
            {
                return new ServiceResult<bool>
                {
                    ErrorCode = 0,
                    ErrorMessage = "",
                    Result = true,
                    Succeed = true
                };
            }
            else
            {
                return new ServiceResult<bool>
                {
                    ErrorCode = 1,
                    ErrorMessage = "LoginFaild",
                    Result = false,
                    Succeed = true
                };
            }
        }
    }
}
