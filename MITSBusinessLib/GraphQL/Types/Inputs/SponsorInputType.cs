using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class SponsorInputType : InputObjectGraphType
    {
        public SponsorInputType()
        {
            Name = "SponsorInput";
            Field<NonNullGraphType<StringGraphType>>("dataDescriptor");
            Field<NonNullGraphType<StringGraphType>>("dataValue");
            Field<NonNullGraphType<StringGraphType>>("firstName");
            Field<NonNullGraphType<StringGraphType>>("lastName");
            Field<NonNullGraphType<StringGraphType>>("organization");
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<IntGraphType>>("registrationTypeId");
            Field<NonNullGraphType<IntGraphType>>("eventId");
        }
    }
}
