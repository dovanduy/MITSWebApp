﻿using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Types;
using MITSBusinessLib.Business;
using MITSBusinessLib.GraphQL.Types;
using MITSBusinessLib.GraphQL.Types.Inputs;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.ResponseModels.WildApricot;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL
{
    public class MITSMutation : ObjectGraphType
    {
        public MITSMutation(IEventsRepository eventsRepo, IDaysRepository daysRepo, ITagsRepository tagRepo, ISectionsRepository sectionsRepo, 
            ISpeakersRepository speakersRepo, IUserRepository userRepo, IWaRepository waRepo, IEventRegistrationBusinessLogic eventRegistrationBusinessLogic)
        {
            Name = "Mutation";


            #region PrintBadge
            Field<PrintBadgeType, int>()
                .Name("printAttendeesBadge")
                //.AuthorizeWith("AdminPolicy")
                .Argument<NonNullGraphType<ListGraphType<PrintBadgeInputType>>>("printBadge",
                    "Print Attendees Badge")
                .ResolveAsync(async context =>
                {
                    var registrationIds = context.GetArgument<PrintBadge>("printBadge");

                    return 12;

                    //return await eventRegistrationBusinessLogic.CheckInAttendee(newCheckIn);

                });
            #endregion

            #region Checkin

            Field<CheckInAttendeeType, CheckInAttendee>()
                .Name("checkInAttendee")
                .AuthorizeWith("CheckinPolicy")
                .Argument<NonNullGraphType<CheckInAttendeeInputType>>("checkInAttendee",
                    "Check in event attendee")
                .ResolveAsync(async context =>
                {
                    var newCheckIn = context.GetArgument<CheckInAttendee>("checkInAttendee");

                    return await eventRegistrationBusinessLogic.CheckInAttendee(newCheckIn);

                });

            #endregion


            #region Registration

            //mutation ProcessRegistration($registration: RegistrationInput!) {
            //    processRegistration(registration: $registration) {
            //        eventRegistrationId,
            //        qrCode
            //    }
            //}

            //{
            //    "registration" :{
            //        "dataDescriptor" : "COMMON.ACCEPT.INAPP.PAYMENT",
            //        "dataValue" : "4354f34f34gfdhsfhfrhdfshs",
            //        "firstName" : "Bob",
            //        "lastName" :"Anderson",
            //        "title" : "CEO, Boeing",
            //        "email" : "enderjs@gmail.com",
            //        "memberId" :"121232",
            //        "memberExpirationDate" : "0118",
            //        "isLifeMember" : false,
            //        "isLocal" : true,
            //        "registrationTypeId" : 4574357,
            //        "eventId" : 3176755
            //    }

            //}

            //Input, output
            Field<RegistrationType, Registration>()
                .Name("processRegistration")
                .Argument<NonNullGraphType<RegistrationInputType>>("registration",
                    "Details to process a new registration")
                .ResolveAsync(async context =>
                {
                    var newRegistration = context.GetArgument<Registration>("registration");
                    return await eventRegistrationBusinessLogic.RegisterAttendee(newRegistration);
                    //return new Registration()
                    //{
                    //    EventRegistrationId = 324234,
                    //    QrCode = "324j2o3kj423ijd23n23ij923jd923jd2938jd2398du2398du2398dj2398"
                    //};
                });


            Field<SponsorType, Sponsor>()
                .Name("processSponsorRegistration")
                .Argument<NonNullGraphType<SponsorInputType>>("sponsor",
                    "Details to process a new registration")
                .ResolveAsync(async context =>
                {
                    var newSponsorRegistration = context.GetArgument<Sponsor>("sponsor");
                    return await eventRegistrationBusinessLogic.RegisterSponsor(newSponsorRegistration);
                    //return new Sponsor()
                    //{
                    //    EventRegistrationId = 324234,
                    //};
                });

            #endregion

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


                }).AuthorizeWith("AdminPolicy");

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


                }).AuthorizeWith("AdminPolicy"); ;

            Field<IntGraphType, int>()
                .Name("deleteSpeaker")
                .AuthorizeWith("AdminPolicy")
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
               eventType
               }
               }
               {
               "event": {
               "mainEventId" :"4334",
               "eventRegistrationType" : "Main"
               }
               }
             */

            Field<EventType, Event>()
                .Name("createEvent")
                .AuthorizeWith("AdminPolicy")
                .Argument<NonNullGraphType<EventInputType>>("event", "event input")
                .ResolveAsync(async context =>
                {
                    try
                    {
                        var newEvent = context.GetArgument<Event>("event");
                        //Is this the best place to put logic for other things..... what other choice do I have....
                        var eventAddedToDb = await eventsRepo.CreateEvent(newEvent);
                        return await waRepo.AddWildApricotEvent(eventAddedToDb);
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
                .AuthorizeWith("AdminPolicy")
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
                .AuthorizeWith("AdminPolicy")
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
