using GraphQL.Types;

namespace MITSBusinessLib.GraphQL.Types.Inputs
{
    public class DayInputType : InputObjectGraphType
    {
        public DayInputType()
        {
            Name = "DayInput";
            Field<NonNullGraphType<DateTimeGraphType>>("agendaDay");

        }
    }
}
