using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.Models
{
    public class EventResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string  Location { get; set; }
        public bool RegistrationEnabled { get; set; }
        public int? RegistrationsLimit { get; set; }
        public EventDetailsResponse Details { get; set; }
  
    }
}
