using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL.Types
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
