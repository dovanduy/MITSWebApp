using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class RegistrationInputType : InputObjectGraphType
    {
        public RegistrationInputType()
        {
            Name = "RegistrationInput";
            Field<NonNullGraphType<StringGraphType>>("dataDescriptor");
            Field<NonNullGraphType<StringGraphType>>("dataValue");
            Field<NonNullGraphType<StringGraphType>>("firstName");
            Field<NonNullGraphType<StringGraphType>>("lastName");
            Field<NonNullGraphType<StringGraphType>>("organization");
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<StringGraphType>("memberId");
            Field<StringGraphType>("memberExpirationDate");
            Field<StringGraphType>("registrationCode");
            Field<BooleanGraphType>("isLifeMember");
            Field<BooleanGraphType>("isLocal");
            Field<NonNullGraphType<IntGraphType>>("registrationTypeId");
            Field<NonNullGraphType<IntGraphType>>("eventId");
        }
    }
}
