using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Order.Repositories
{
    public interface IOrderService
    {
        ServiceResult<string> Register(OrderModel order);
        ServiceResult<OrderDetails> GetOrderDetails(OrderModel order);
        ServiceResult<string> VerifyPayment(OrderModel order);
        ServiceResult<bool> RefoundPayment(OrderModel order);
    }
}
