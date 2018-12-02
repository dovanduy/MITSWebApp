using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Speaker
    {

        public Speaker()
        {
            //This makes it so you don't have to instantiate the list when you want to add items. 
            SpeakerSections = new List<SectionSpeaker>();
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Bio { get; set; }
        public Guid ImageName { get; set; }
        public ICollection<SectionSpeaker> SpeakerSections { get; set; } 

    }
}
