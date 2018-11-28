using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public Guid ImageName { get; set; }
    }
}
