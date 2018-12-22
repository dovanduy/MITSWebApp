using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.Models
{
    public class EventRegistrationTypesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string Availability { get; set; }
        public string RegistrationCode { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableThrough { get; set; }
    }
}
