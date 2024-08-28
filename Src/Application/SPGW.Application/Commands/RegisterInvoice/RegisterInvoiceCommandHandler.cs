using Microsoft.AspNetCore.Http;
using SPGW.Application.Common;
using SPGW.Domain.Common;
using SPGW.Domain.Order.Entities;
using SPGW.Domain.Order.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Application.Commands.RegisterInvoice
{
    public class RegisterInvoiceCommandHandler : ICommandHandler<RegisterInvoiceCommand, ServiceResult<string>>
    {
        IOrderService _orderService;
        HttpContext _httpContext;

        public RegisterInvoiceCommandHandler(IOrderService orderService, IHttpContextAccessor accessor)
        {
            _orderService = orderService;
            _httpContext = accessor.HttpContext;
        }

        public Task<ServiceResult<string>> Handle(RegisterInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new ServiceResult<string> { ErrorCode = 0, ErrorMessage = "عملیات با موفقیت به اتمام رسید" };
            try
            {
                string authToken = _httpContext.Request.Headers["Authorization"].ToString();
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                request.CustomerCode = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                result = _orderService.Register(request);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = "خطا در دریافت اطلاعات کاربری مشتری";
            }

            return Task.FromResult(result);
        }
    }
}
