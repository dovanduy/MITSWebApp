using GraphQL.Types;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class WaRegistrationType : ObjectGraphType<WildApricotRegistrationType>
    {
        public WaRegistrationType()
        {
            Field(rt => rt.RegistrationTypeId);
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
