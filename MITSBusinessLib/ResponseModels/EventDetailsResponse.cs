using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.Models
{
    public class EventDetailsResponse
    {
        public string DescriptionHtml { get; set; }
        public List<EventRegistrationTypesResponse> RegistrationTypes { get; set; }
        
    }
}
