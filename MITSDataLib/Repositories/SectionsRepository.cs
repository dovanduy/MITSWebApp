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
    public class SectionsRepository : ISectionsRepository
    {
        private readonly MITSContext _context;

        public SectionsRepository(MITSContext context)
        {
            _context = context;
        }

        public async Task<List<Section>> getSectionsByDayId(int id)
        {
            return await _context.Sections
                .Where(section => section.DayId == id)
                .ToListAsync();
    
        }

        public async Task<List<Section>> getSectionsByTagId(int id)
        {
            return await _context.SectionsTags
                .Where(st => st.TagId == id)
                .Select(st => st.Section)
                .ToListAsync();
        }
    }
}
