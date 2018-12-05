using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL
{
    public class MITSSchema : Schema
    {
        public MITSSchema(IDependencyResolver resolver) : base (resolver)
        {
            Query = resolver.Resolve<MITSQuery>();
            Mutation = resolver.Resolve<MITSMutation>();
        }
    }
}
