using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MITSDataLib.Repositories
{
    class SectionsRepository : ISectionsRepository
    {
        private readonly MITSContext context;

        public SectionsRepository(MITSContext context)
        {
            this.context = context;
        }

        public async Task<List<Section>> getSectionsByDayId(int id)
        {
            return await context.Sections
                .Where(section => section.DayId == id)
                .ToListAsync();
    
        }
    }
}
