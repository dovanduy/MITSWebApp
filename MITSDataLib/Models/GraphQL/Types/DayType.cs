using GraphQL.Types;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models.GraphQL.Types
{
    public class DayType : ObjectGraphType<Day>
    {
        public DayType(ISectionsRepository sectionsRepo)
        {
            Field(d => d.Id);
            Field(d => d.AgendaDay);
            Field<ListGraphType<SectionType>, List<Section>>()
                .Name("Sections")
                .ResolveAsync(context => sectionsRepo.GetSectionsByDayIdAsync(context.Source.Id));
        }
    }
}
