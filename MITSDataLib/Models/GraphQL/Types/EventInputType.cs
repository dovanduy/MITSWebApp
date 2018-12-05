using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class EventInputType: InputObjectGraphType
    {
        public EventInputType()
        {
            Name = "EventInput";
            Field<NonNullGraphType<IntGraphType>>("eventId");
            Field<NonNullGraphType<BooleanGraphType>>("isSponsor");
        }
    }
}
