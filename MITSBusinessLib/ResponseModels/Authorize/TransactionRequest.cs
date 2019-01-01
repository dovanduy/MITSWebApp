using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class TransactionRequest
    {
        public string TransactionType { get; set; }
        public string Amount { get; set; }
        public Payment Payment { get; set; }
        public Order Order { get; set; }
        public LineItems LineItems { get; set; }

        //public BillTo BillTo { get; set; }

    }
}
