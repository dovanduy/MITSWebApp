using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class RegistrationType: ObjectGraphType<Registration>
    {
        public RegistrationType()
        {
            Field(r => r.DataDescriptor);
            Field(r => r.DataValue);
            Field(r => r.FirstName);
            Field(r => r.LastName);
            Field(r => r.Title);
            Field(r => r.Email);
            Field(r => r.MemberId);
            Field(r => r.MemberExpirationDate);
            Field(r => r.IsLifeMember);
            Field(r => r.IsLocal);
            Field(r => r.RegistrationTypeId);
            Field(r => r.EventId);
            Field(r => r.EventRegistrationId);
            Field(r => r.QrCode);
        }
    }
}
