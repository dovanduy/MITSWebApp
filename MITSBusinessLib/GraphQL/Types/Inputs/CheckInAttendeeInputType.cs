using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class CheckInAttendeeInputType : InputObjectGraphType
    {
        public CheckInAttendeeInputType()
        {
            Name = "CheckInAttendeeInput";
            Field<NonNullGraphType<IntGraphType>>("registrationId");
        }
    }
}
