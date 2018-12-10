using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using MITSDataLib.Models.GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL
{
    
    public class MITSQuery : ObjectGraphType
    {
        
        public MITSQuery(IEventsRepository eventsRepo, IDaysRepository daysRepo, ISpeakersRepository speakerRepo, ITagsRepository tagRepo, IUserRepository userRepo, ISectionsRepository sectionsRepo)
        {
            Name = "query";

            #region Event

            Field<EventType>(
                "event",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
                resolve: context => eventsRepo.GetEventByIdAsync(context.GetArgument<int>("id"))

                );

            //this.AuthorizeWith("AdminPolicy");
            Field<ListGraphType<EventType>>(
                "events",
                resolve: context => eventsRepo.GetEventsAsync()
                )
                //.AuthorizeWith("AdminPolicy")
                ;

            #endregion

            #region Day
            //Field<ListGraphType<DayType>>(
            //    "days",
            //    resolve: context => daysRepo.GetDays()
            //    );


            Field<ListGraphType<DayType>, List<Day>>()
                .Name("days")
                .ResolveAsync(context => daysRepo.GetDaysAsync());

            #endregion

            #region Section

            Field<ListGraphType<SectionType>, List<Section>>()
                .Name("sections")
                .ResolveAsync(context => sectionsRepo.GetSectionsAsync());
            

            #endregion

            #region Speaker

            Field<ListGraphType<SpeakerType>, List<Speaker>>()
                .Name("speakers")
                .ResolveAsync(context => speakerRepo.GetSpeakersAsync() );

            Field<SpeakerType, Speaker>()
                .Name("speaker")
                .Argument<NonNullGraphType<IntGraphType>>("speakerId", "Id of speaker to get")
                .ResolveAsync(context => speakerRepo.GetSpeakerByIdAsync(context.GetArgument<int>("speakerId")));

            #endregion

            #region Tag

            Field<ListGraphType<TagType>, List<Tag>>()
                .Name("tags")
                .ResolveAsync(context => tagRepo.GetTagsAsync());

            #endregion

            #region User

            Field<ListGraphType<UserType>, List<User>>()
                .Name("users")
                .ResolveAsync(context => userRepo.GetUsersAsync());

            #endregion

        }



    }
}
