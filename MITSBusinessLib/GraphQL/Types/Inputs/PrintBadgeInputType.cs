using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class PrintBadgeInputType : InputObjectGraphType
    {
        public PrintBadgeInputType()
        {
            Name = "PrintBadgeInput";
            Field<NonNullGraphType<ListGraphType<IntGraphType>>>("registrationIds");
        }
    }
}
