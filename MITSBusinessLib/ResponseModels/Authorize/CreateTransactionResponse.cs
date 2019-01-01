using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class CreateTransactionResponse
    {
        public TransactionResponse TransactionResponse { get; set; }
        public CreateTransactionResponseMessage Messages { get; set; }
    }
}
