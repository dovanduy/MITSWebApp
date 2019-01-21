using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace MITSDataLib.Models
{
    public class WildApricotEvent
    {
        public int Id { get; set; }
        //public int WaId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public int RegistrationsLimit { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public List<WildApricotRegistrationType> WaRegistrationTypes { get; set; }
        

    }
}
