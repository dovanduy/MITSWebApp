using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class SectionTag
    {
        public int SectionId { get; set; }
        public int TagId { get; set; }

        public Section Section { get; set; }
        public Tag Tag { get; set; }
    }
}
