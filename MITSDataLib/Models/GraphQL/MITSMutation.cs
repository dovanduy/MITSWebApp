using GraphQL.Types;
using MITSDataLib.Models.GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;

namespace MITSDataLib.Models.GraphQL
{
    public class MITSMutation : ObjectGraphType
    {
        public MITSMutation(IEventsRepository eventsRepo, IDaysRepository daysRepo, ITagsRepository tagRepo, ISectionsRepository sectionsRepo, ISpeakersRepository speakersRepo, IUserRepository userRepo)
        {
            Name = "Mutation";

            #region Event                   

            /*
             *mutation ($event: EventInput!)  {
               createEvent(event:$event){
               id
               mainEventId
               isSponsor
               }
               }
               {
               "event": {
               "mainEventId" :"4334",
               "isSponsor" : "False"
               }
               }
             */

            Field<EventType>(
                "createEvent",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<EventInputType>> {Name = "event" }
                    ),
                resolve: context => 
                {
                    try
                    {
                        var newEvent = context.GetArgument<Event>("event");
                        return eventsRepo.CreateEvent(newEvent);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    

                });

            #endregion

            #region Day
            /*
             Create Day
             *mutation ($day: DayInput!)  {
               createEvent(day:$day){
               id
               agendaDay
               }
               }
             *
             * {
               "day": {
               "agendaDay" : "2018-10-09"
               }
               }
             */
            Field<DayType, Day>()
                .Name("createDay")
                .Argument<NonNullGraphType<DayInputType>>("day", "day input")
                .ResolveAsync(context =>
                {
                    try
                    {
                        var newDay = context.GetArgument<Day>("day");
                        return daysRepo.CreateDayAsync(newDay);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                });

            /*deleteDay 
             *mutation ($dayId: Int!)  {
             *  deleteDay(dayId:$dayId){
             *      id
             *      agendaDay
             *  }
             *}
             *
             * {
             *   "dayId" : "3"
             * }
             */

            Field<IntGraphType, List<Day>>()
                .Name("deleteDay")
                .Argument<NonNullGraphType<IntGraphType>>("dayId", "Id of Day to delete")
                .ResolveAsync(context =>
                {
                    try
                    {
                        return daysRepo.DeleteDayAsync(context.GetArgument<int>("dayId"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        context.Errors.Add(new ExecutionError(e.Message));
                        return null;
                    }
                });


            #endregion
        }


    }
}
