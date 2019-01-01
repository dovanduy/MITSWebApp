using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
