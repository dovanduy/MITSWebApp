using GraphQL.Types;
using MITSBusinessLib.ResponseModels.WildApricot;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.GraphQL.Types
{
    public class CheckInAttendeeType : ObjectGraphType<CheckInAttendee>
    {
        public CheckInAttendeeType()
        {
            Field(cia => cia.CheckedIn);
            Field(cia => cia.RegistrationId);
            Field(cia => cia.Status);
        }
    }
}
