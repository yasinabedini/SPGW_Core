using SPGW.Domain.Common;
using SPGW.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Psp.Repositories
{
    public interface IIRKService
    {
        ServiceResult<object> GetPSPDetails(string orderCode, string payerName, string payerMobile);
        ServiceResult<RedirectToMerchantModel> CheckTransactionResult(string token, string responseCode, string RequestId, string paymentId, string retrievalReferenceNumber, string systemTraceAuditNumber);
        IRKVerifyPaymentResult CallVerifyPaymentMethod(IRKVerifyPaymentParamenterModel invoice);
        IRKRefoundPaymentResult CallRefoundPaymentMethod(IRKRefoundPaymentParamenterModel invoice);
    }
}
