using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MITSDataLib.Models
{
    public class WildApricotRegistration
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }
        //Populated from Availability
        public bool CodeRequired { get; set; }
        public string RegistrationCode { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableThrough { get; set; }
        public string Name { get; set; }

        public int WaEventId { get; set; }
        public WildApricotEvent WaEvent { get; set; }

    }
}
