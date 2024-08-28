using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Helpers
{
    public class InfraSetting
    {
        public bool ClientValidationEnabled { get; set; }
        public string IRKGateWayRedirectAddress { get; set; }
        public bool UnobtrusiveJavaScriptEnabled { get; set; }
    }
}
