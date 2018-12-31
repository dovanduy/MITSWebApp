using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
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
            Field<ListGraphType<WaRegistrationType>, List<WildApricotRegistrationType>>()
                .Name("types")
                .ResolveAsync(context => eventsRepo.GetWaRegistrationTypesByWaEventId(context.Source.Id));
        }
    }
}
