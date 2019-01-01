using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class NewContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public bool RecreateInvoice { get; set; }
    }
}
