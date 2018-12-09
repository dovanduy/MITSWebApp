using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class TagType : ObjectGraphType<Tag>
    {
        public TagType(ISectionsRepository sectionRepo)
        {
            Field(t => t.Id);
            Field(t => t.Name);
            Field<ListGraphType<SectionType>, List<Section>>()
                .Name("Sections")
                .ResolveAsync(context => sectionRepo.getSectionsByTagId(context.Source.Id));
        }
    }
}
