using GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class SectionType : ObjectGraphType<Section>
    {
        public SectionType(ISpeakerRepository speakerRepo)
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
                .ResolveAsync(context => 
                {
                    return speakerRepo.GetSpeakersBySectionIdAsync(context.Source.Id);
                } );

        }
    }
}
