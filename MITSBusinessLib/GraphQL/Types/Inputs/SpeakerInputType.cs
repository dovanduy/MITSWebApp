using GraphQL.Types;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class SpeakerInputType : InputObjectGraphType
    {
        public SpeakerInputType()
        {
            Name = "SpeakerInput";
            Field<IntGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("firstName");
            Field<NonNullGraphType<StringGraphType>>("lastName");
            Field<NonNullGraphType<StringGraphType>>("bio");
            Field<NonNullGraphType<StringGraphType>>("title");
        }
        
    }
}
