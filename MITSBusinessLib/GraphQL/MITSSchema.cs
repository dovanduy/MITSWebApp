using GraphQL;
using GraphQL.Types;

namespace MITSBusinessLib.GraphQL
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
