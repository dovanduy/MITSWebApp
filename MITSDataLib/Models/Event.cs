using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int WaEventId { get; set; }
        public WildApricotEvent WaEvent { get; set; }

    }
}
