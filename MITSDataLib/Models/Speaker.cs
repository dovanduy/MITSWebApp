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

        //Update with speaker is a panel member or not

        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Section Description cannot be longer than 500 characters.")]
        public string Bio { get; set; }

        public Guid ImageName { get; set; }

        [Required]
        public bool IsPanelist { get; set; }
        public ICollection<SectionSpeaker> SpeakerSections { get; set; } 

    }
}
