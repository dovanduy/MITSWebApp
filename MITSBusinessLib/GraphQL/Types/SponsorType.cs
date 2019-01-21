using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class SponsorType: ObjectGraphType<Sponsor>
    {
        public SponsorType()
        {
            Field(r => r.DataDescriptor);
            Field(r => r.DataValue);
            Field(r => r.FirstName);
            Field(r => r.LastName);
            Field(r => r.Organization);
            Field(r => r.Email);      
            Field(r => r.RegistrationTypeId);
            Field(r => r.EventId);
            Field(r => r.EventRegistrationId);
        }
    }
}
