using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class SpeakerType : ObjectGraphType<Speaker>
    {
        public SpeakerType(ISectionsRepository sectionsRepo)
        {
            Field(s => s.Id);
            Field(s => s.FirstName);
            Field(s => s.LastName);
            Field(s => s.Bio);
            Field(s => s.IsPanelist);
            Field(s => s.Title);
            Field<ListGraphType<SectionType>, List<Section>>()
                .Name("sections")
                .ResolveAsync(context => sectionsRepo.GetSectionsBySpeakerIdAsync(context.Source.Id));
            //Field<GuidGraphType>("imageName", resolve: context => context.Source.ImageName);
            //Remember to mutate back to a guid type to store in the database.
            //Field(s => s.ImageName, type: typeof (StringGraphType));
        }
    }
}
