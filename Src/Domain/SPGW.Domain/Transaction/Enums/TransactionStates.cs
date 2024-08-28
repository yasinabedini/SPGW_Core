using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Transaction.Enums
{
    public enum TransactionStates
    {
        Register = 0,
        PaymentFail = 1,
        PaymentSuccess = 2,
        PaymentRefunded = 3,
        PaymentVerified = 4
    }
}
