using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Section
    {
        public Section()
        {
            //This makes it so you don't have to instantiate the list when you want to add items. 
            SectionSpeakers = new List<SectionSpeaker>();
            SectionTags = new List<SectionTag>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Section name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Section Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        
        public string SlideUrl { get; set; }

        
        public bool RestrictSlide { get; set; }

        [Required]
        public bool IsPanel { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int DayId { get; set; }
        public Day Day { get; set; }

        public List<SectionTag> SectionTags { get; set; }
        public List<SectionSpeaker> SectionSpeakers { get; set; } 

    }
}
