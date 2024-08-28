using Microsoft.EntityFrameworkCore;
using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Psp.Repositories
{
    public interface IPNAService
    {
        //ServiceResult<WSContext> Login(string username, string password);
        //ServiceResult<WSContext> Login(string username);

        ServiceResult<object> GetPSPDetails(string orderCode, string payerName, string payerMobile);

        ServiceResult<bool> CheckTransactionResult(string ResNum);

        ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string ResNum, string token, string RefNum, decimal? MID);

        ServiceResult<bool> RefoundPaymentMethod(Order.Entities.Order order);
        ServiceResult<bool> VerifyPaymentMethod(Order.Entities.Order order);

        //ServiceResult<object> CheckTransactionResult(string tref, string iN, string iD);
        //PEPCheckTransactionResult CallCheckTransactionResultMethodByTref(string tref);
        //PEPCheckTransactionResult CallCheckTransactionResultMethodByiNiD(string iN, string iD, string mc, string tc);
        //PEPVerifyPaymentResult CallVerifyPaymentMethod(PEPVerifyPaymentParamenterModel invoice);
        //PEPRefoundPaymentResult CallRefoundPaymentMethod(PEPRefoundPaymentParamenterModel invoice);
    }
}
