using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.ResponseModels.WildApricot
{
    public class EventRegistrationCheckIn
    {
        public int RegistrationId { get; set; }
        public bool CheckedIn { get; set; }
    }
}
