using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Section
    {
        public Section()
        {
            //This makes it so you don't have to instantiate the list when you want to add items. 
            SectionSpeakers = new List<SectionSpeaker>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SlideUrl { get; set; }
        public bool RestrictSlide { get; set; }
        public bool IsPanel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Day Day { get; set; }
        public ICollection<SectionSpeaker> SectionSpeakers { get; set; } 

    }
}
