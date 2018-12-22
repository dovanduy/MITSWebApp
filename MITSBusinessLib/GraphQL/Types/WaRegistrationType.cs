using GraphQL.Types;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class WaRegistrationType : ObjectGraphType<WildApricotRegistration>
    {
        public WaRegistrationType()
        {
            Field(rt => rt.Name);
            Field(rt => rt.Description);
            Field(rt => rt.AvailableFrom);
            Field(rt => rt.AvailableThrough);
            Field(rt => rt.BasePrice);
            Field(rt => rt.RegistrationCode, true);
            Field(rt => rt.IsEnabled);         
        }
    }
}
