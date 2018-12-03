using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public int WaEventId { get; set; }
        [Required]
        public bool IsSponsor { get; set; }
        public WildApricotEvent WaEvent { get; set; }

    }
}
