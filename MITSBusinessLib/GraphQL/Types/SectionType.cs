using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class SectionType : ObjectGraphType<Section>
    {
        public SectionType(ISpeakersRepository speakerRepo, ITagsRepository tagsRepo)
        {
            Field(s => s.Id);
            Field(s => s.Name);
            Field(s => s.Description);
            Field(s => s.SlideUrl);
            Field(s => s.RestrictSlide);
            Field(s => s.IsPanel);
            Field(s => s.StartDate);
            Field(s => s.EndDate);
            Field<ListGraphType<SpeakerType>, List<Speaker>>()
                .Name("speakers")
                .ResolveAsync(context => speakerRepo.GetSpeakersBySectionIdAsync(context.Source.Id));
            Field<ListGraphType<TagType>, List<Tag>>()
                .Name("tags")
                .ResolveAsync(context => tagsRepo.GetTagsBySectionIdAsync(context.Source.Id));

        }
    }
}
