using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SlideUrl { get; set; }
        public bool RestrictSlide { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Speaker> Speakers { get; set; }
        public Day Day { get; set; }

    }
}
