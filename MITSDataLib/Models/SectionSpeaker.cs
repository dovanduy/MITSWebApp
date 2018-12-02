using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class SectionSpeaker
    {
        public int SectionId { get; set; }
        public int SpeakerId { get; set; }

        public Section Section { get; set; }
        public Speaker Speaker { get; set; }
    }
}
