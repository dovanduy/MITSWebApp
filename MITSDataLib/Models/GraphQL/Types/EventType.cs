using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class EventType : ObjectGraphType<Event>
    {
        public EventType()
        {
            
            Field(e => e.Id);
            Field(e => e.IsSponsor);
            Field(e => e.EventId);
            //Add Field for WAEvent after it is completed
        }
    }
}
