using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using MITSBusinessLib.Business;
using MITSBusinessLib.GraphQL.Types;
using MITSBusinessLib.GraphQL.Types.Inputs;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL
{
    public class MITSMutation : ObjectGraphType
    {
        public MITSMutation(IEventsRepository eventsRepo, IDaysRepository daysRepo, ITagsRepository tagRepo, ISectionsRepository sectionsRepo, 
            ISpeakersRepository speakersRepo, IUserRepository userRepo, IWaRepository waRepo)
        {
            Name = "Mutation";

            #region Speaker

            Field<SpeakerType>(
                "createSpeaker",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<SpeakerInputType>> { Name = "speaker" }
                ),
                resolve: context =>
                {
                    try
                    {
                        var newSpeaker = context.GetArgument<Speaker>("speaker");
                        return speakersRepo.CreateSpeakerAsync(newSpeaker);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }


                });

            Field<SpeakerType>(
                "updateSpeaker",

                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<SpeakerInputType>> { Name = "speaker" }
                ),
                resolve: context =>
                {
                    try
                    {
                        var newSpeakerValues = context.GetArgument<Speaker>("speaker");
                        return speakersRepo.UpdateSpeakerAsync(newSpeakerValues);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }


                });

            Field<IntGraphType, int>()
                .Name("deleteSpeaker")
                .Argument<NonNullGraphType<IntGraphType>>("speakerId", "Id of Speaker to delete")
                .ResolveAsync(context =>
                {
                    try
                    {
                        return speakersRepo.DeleteSpeakerAsync(context.GetArgument<int>("speakerId"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        context.Errors.Add(new ExecutionError(e.Message));
                        return null;
                    }
                });

            #endregion

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

            Field<EventType, Event>()
                .Name("createEvent")
                .Argument<NonNullGraphType<EventInputType>>("event", "event input")
                .ResolveAsync(async context =>
                {
                    try
                    {
                        var newEvent = context.GetArgument<Event>("event");
                        //Is this the best place to put logic for other things..... what other choice do I have....
                        var eventAddedToDB = await eventsRepo.CreateEvent(newEvent);
                        return await waRepo.AddWildApricotEvent(eventAddedToDB);
                       ;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                });

            //Field<EventType>(
            //    "createEvent",
            //    arguments: new QueryArguments(
            //        new QueryArgument<NonNullGraphType<EventInputType>> {Name = "event" }
            //        ),
            //    resolve: context => 
            //    {
            //        try
            //        {
            //            var newEvent = context.GetArgument<Event>("event");
            //            //Is this the best place to put logic for other things..... what other choice do I have....
            //            await waRepo.AddWildApricotEvent(newEvent);
            //            return eventsRepo.CreateEvent(newEvent);
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e);
            //            throw;
            //        }
                    

            //    });

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
