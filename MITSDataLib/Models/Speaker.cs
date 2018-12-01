using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Bio { get; set; }
        public Guid ImageName { get; set; }
    }
}
