using SPGW.Application.Common;
using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Application.Commands.RegisterInvoice
{
    public class RegisterInvoiceCommand:OrderModel,ICommand<ServiceResult<string>>
    {
    }
}
