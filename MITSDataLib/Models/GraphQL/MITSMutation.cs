using GraphQL.Types;
using MITSDataLib.Models.GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL
{
    public class MITSMutation : ObjectGraphType
    {
        public MITSMutation(IEventsRepository eventRepo)
        {
            Name = "Mutation";

            Field<EventType>(
                "createEvent",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<EventInputType>> {Name = "event" }
                    ),
                resolve: context => 
                {
                    var newEvent = context.GetArgument<Event>("event");
                    return eventRepo.AddEvent(newEvent);

                });
        }

        
    }
}
