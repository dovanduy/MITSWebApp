using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.Authorize
{
    public class TransactionResponse
    {
        public string ResponseCode { get; set; }
        public string AuthCode { get; set; }
        public string AvsResultCode { get; set; }
        public string CvvResultCode { get; set; }
        public string CavvResultCode { get; set; }
        public string TransId { get; set; }
        public string RefTransId { get; set; }
        public string TransHash { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public TransactionResponseMessage[] Messages { get; set; }
        public TransactionResponseErrors[] Errors { get; set; }
        public string TransHashSha2 { get; set; }
        public string SupplementalDataQualificationIndicator { get; set; }


    }

    
}
