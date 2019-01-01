using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class NewEventRegistration
    {
        public WaEvent Event { get; set; }
        public WaContact Contact { get; set; }
        public int RegistrationTypeId { get; set; }
        public bool IsCheckedIn { get; set; }
        public List<RegistrationField> RegistrationFields { get; set; }
        public int ShowToPublic { get; set; }
        public string RegistrationDate { get; set; }
        public string Memo { get; set; }
        public bool RecreateInvoice { get; set; }
    }
}
