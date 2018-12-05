using GraphQL.Types;
using MITSDataLib.Models.GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace MITSDataLib.Models
{
    public class MITSQuery : ObjectGraphType
    {
        public MITSQuery(IEventsRepository eventRepo)
        {
            Field<EventType>(
                "event",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
                resolve: context => eventRepo.GetEvent(context.GetArgument<int>("id"))

                );

            Field<ListGraphType<EventType>>(
                "events",
                resolve: context => eventRepo.GetEvents()
                ); 
            
        }

       
       
    }
}
