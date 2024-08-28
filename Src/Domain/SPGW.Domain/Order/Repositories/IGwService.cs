using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Order.Repositories
{
    public interface IGWService
    {
        ServiceResult<PayOrderModel> GetOrderInformation(string orderCode);
    }
}
