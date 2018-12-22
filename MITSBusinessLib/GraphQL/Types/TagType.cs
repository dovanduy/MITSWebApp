using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType(ISectionsRepository sectionRepo)
        {
            Field(t => t.Id);
            Field(t => t.Name);
            Field<ListGraphType<SectionType>, List<Section>>()
                .Name("Sections")
                .ResolveAsync(context => sectionRepo.GetSectionsByTagIdAsync(context.Source.Id));
        }
    }
}
