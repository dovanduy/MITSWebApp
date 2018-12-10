using System.Collections.Generic;
using GraphQL.Types;
using MITSDataLib.Models.GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL
{
    
    public class MITSQuery : ObjectGraphType
    {
        
        public MITSQuery(IEventsRepository eventsRepo, IDaysRepository daysRepo, ISpeakersRepository speakerRepo, ITagsRepository tagRepo, IUserRepository userRepo)
        {
            Name = "query";

            #region Event

            Field<EventType>(
                "event",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
                resolve: context => eventsRepo.GetEvent(context.GetArgument<int>("id"))

                );

            //this.AuthorizeWith("AdminPolicy");
            Field<ListGraphType<EventType>>(
                "events",
                resolve: context => eventsRepo.GetEvents()
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
                .ResolveAsync(context =>
                {
                    return daysRepo.GetDays();
                });

            #endregion

            #region Speaker

            Field<ListGraphType<SpeakerType>, List<Speaker>>()
                .Name("speakers")
                .ResolveAsync(context => speakerRepo.GetSpeakersAsync() );

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
