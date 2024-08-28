using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPGW.Application.Commands.RegisterInvoice;
using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System.Text;

namespace SPGW.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterInvoice : ControllerBase
    {
        private readonly ISender _sender;

        public RegisterInvoice(ISender sender)
        {
            _sender = sender;
        }

        //[CustomAuthorize]
        public async Task<ServiceResult<string>> Post([FromBody] RegisterInvoiceCommand command)
        {
            var result =await _sender.Send(command);

            if (!result.Succeed)
            {
                Response.StatusCode = 400;
                return result;
            }

            return result;
        }
    }
}
