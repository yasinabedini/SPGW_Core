using SPGW.Domain.Common;
using SPGW.Domain.Customer.Entities;

namespace SPGW.Domain.Customer.Repositories
{

    public interface IUserManagerService
    {
        ServiceResult<bool> HasAccess(string username, string password);
    }
}