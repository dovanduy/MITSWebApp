using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Sponsor
    {
        public string DataDescriptor { get; set; }
        public string DataValue { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public int RegistrationTypeId { get; set; }
        public int EventId { get; set; }
        public int EventRegistrationId { get; set; }
    }
}
