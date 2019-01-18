using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Registration
    {
        public string DataDescriptor { get; set; }
        public string DataValue { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string MemberId { get; set; }
        public string MemberExpirationDate { get; set; }
        public bool IsLifeMember { get; set; }
        public bool IsLocal { get; set; }
        public string RegistrationCode { get; set; }
        public int RegistrationTypeId { get; set; }
        public int EventId { get; set; }
        public int EventRegistrationId { get; set; }
        public string QrCode { get; set; }
    }
}
