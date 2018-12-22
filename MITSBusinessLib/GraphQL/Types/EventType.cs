using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class EventType : ObjectGraphType<Event>
    {
        public EventType(IEventsRepository eventsRepo)
        {
            
            Field(e => e.Id);
            Field(e => e.IsSponsor);
            Field(e => e.MainEventId);
            Field<ListGraphType<WaEventType>, List<WildApricotEvent>>()
                .Name("WaEvent")
                .ResolveAsync(context => eventsRepo.GetWaEventByEventId(context.Source.Id));
        }
    }
}
