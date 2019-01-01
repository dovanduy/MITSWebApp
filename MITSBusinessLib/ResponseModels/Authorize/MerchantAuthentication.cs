using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class MerchantAuthentication
    {
        public string Name { get; set; }
        public string TransactionKey { get; set; }
    }
}
