using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class WaEventType : ObjectGraphType<WildApricotEvent>
    {
        public WaEventType(IEventsRepository eventsRepo)
        {
            Field(e => e.Id);
            Field(e => e.Name);
            Field(e => e.Description);
            Field(e => e.IsEnabled);
            Field(e => e.Location);
            Field(e => e.StartDate);
            Field(e => e.EndDate);
            Field<ListGraphType<WaRegistrationType>, List<WildApricotRegistration>>()
                .Name("types")
                .ResolveAsync(context => eventsRepo.GetWaRegistrationTypesByWaEventId(context.Source.Id));
        }
    }
}
