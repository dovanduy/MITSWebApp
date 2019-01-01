using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class CreateTransactionRequest
    {
        public MerchantAuthentication MerchantAuthentication { get; set; }
        public TransactionRequest TransactionRequest { get; set; }
    }
}
