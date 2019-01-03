using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class CheckInAttendee
    {
        public int RegistrationId { get; set; }
        public bool CheckedIn { get; set; }
        public string Status { get; set; }
    }
}
