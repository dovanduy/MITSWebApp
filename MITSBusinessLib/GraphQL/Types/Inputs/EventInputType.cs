using GraphQL.Types;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class EventInputType: InputObjectGraphType
    {
        public EventInputType()
        {
            Name = "EventInput";
            Field<NonNullGraphType<IntGraphType>>("mainEventId");
            Field<NonNullGraphType<BooleanGraphType>>("isSponsor");
        }
    }
}
