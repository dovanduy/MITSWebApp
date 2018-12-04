using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Day
    {
        public int Id { get; set; }

        [Required]
        public DateTime AgendaDay { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
