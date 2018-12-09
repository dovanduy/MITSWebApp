using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Tag
    {
        public Tag()
        {
            TagSections = new List<SectionTag>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Tag cannot be longer than 50 characters.")]
        public string Name { get; set; }

        public List<SectionTag> TagSections { get; set; }
    }
}
