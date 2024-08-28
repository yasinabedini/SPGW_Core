using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.DTOs
{
    public class BPMPayRequestModel
    {
        public long terminalId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderId { get; set; }
        public long amount { get; set; }
        public string localDate { get; set; }
        public string localTime { get; set; }
        public string additionalData { get; set; }
        public string callBackUrl { get; set; }
        public string payerId { get; set; }
        public string mobileNo { get; set; }
        public string encPan { get; set; }
        public string panHiddenMode { get; set; }
        public string cartItem { get; set; }
        public string enc { get; set; }
    }

    public class BPMVerifyRequestModel
    {
        public long terminalId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public long orderId { get; set; }
        public long saleOrderId { get; set; }
        public long saleRefrenceId { get; set; }
    }

    public class BPMPayResponseModel
    {
        public long ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RefId { get; set; }
    }

    public class BPMIPGBack
    {
        public string RefId { get; set; }//59E419419B189191
        public long ResCode { get; set; }//0
        public string SaleOrderId { get; set; }//637760860118121920
        public string SaleReferenceId { get; set; }//204965052649
        public string CardHolderInfo { get; set; }//91F51CE2DBB175F4051EE09A43CCAD805889B4C8D5912977D487F062C516CA3B
        public string CardHolderPan { get; set; }//606373******9298
        public long FinalAmount { get; set; }//55000
    }
}