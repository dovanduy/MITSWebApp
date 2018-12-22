using System.Collections.Generic;
using GraphQL.Types;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Models;

namespace MITSBusinessLib.GraphQL.Types
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
