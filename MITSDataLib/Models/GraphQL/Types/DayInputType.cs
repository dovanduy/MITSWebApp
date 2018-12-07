using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    class DayInputType : InputObjectGraphType
    {
        public DayInputType()
        {
            Name = "DayInput";
            Field<NonNullGraphType<DateTimeGraphType>>("AgendaDay");

        }
    }
}
