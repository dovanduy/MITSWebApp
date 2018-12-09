using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;

namespace MITSDataLib.Models.GraphQL.Types
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
