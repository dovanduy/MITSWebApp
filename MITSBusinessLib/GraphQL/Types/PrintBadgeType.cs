using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class PrintBadgeType : ObjectGraphType<PrintBadge>
    {
        public PrintBadgeType()
        {
            Field(pb => pb.Name);
        }
    }
}
