using GraphQL.Authorization;
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
        
        public MITSQuery(IEventsRepository eventsRepo, IDaysRepository daysRepo)
        {
            Field<EventType>(
                "event",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
                resolve: context => eventsRepo.GetEvent(context.GetArgument<int>("id"))

                );

            this.AuthorizeWith("AdminPolicy");
            Field<ListGraphType<EventType>>(
                "events",
                resolve: context => eventsRepo.GetEvents()
                );


            Field<ListGraphType<DayType>, List<Day>>()
                .Name("Days")
                .ResolveAsync(context =>
                {
                    return daysRepo.GetDays();
                });
            
        }

       
       
    }
}
