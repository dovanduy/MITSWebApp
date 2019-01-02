using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class EventCheckInAudit
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CheckInAt { get; set; }

    }
}
