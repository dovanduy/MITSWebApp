using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MITSDataLib.Models
{
    public class WildApricotToken
    {
        public int Id { get; set; }
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public DateTime? TokenExpires { get; set; }
    }
}
