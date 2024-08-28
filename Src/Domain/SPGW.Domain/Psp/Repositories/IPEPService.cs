using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Psp.Repositories
{
    public interface IPEPService
    {
        ServiceResult<object> GetPSPDetails(string orderCode, string payerName, string payerMobile);
        ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string tref, string iN, string iD);
        PEPCheckTransactionResult CallCheckTransactionResultMethodByTref(string tref);
        PEPCheckTransactionResult CallCheckTransactionResultMethodByiNiD(string iN, string iD, string mc, string tc);
        PEPVerifyPaymentResult CallVerifyPaymentMethod(PEPVerifyPaymentParamenterModel invoice);
        PEPRefoundPaymentResult CallRefoundPaymentMethod(PEPRefoundPaymentParamenterModel invoice);
    }
}
