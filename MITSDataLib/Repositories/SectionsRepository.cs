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

        public async Task<List<Section>> GetSectionsAsync()
        {
            return await _context.Sections
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Section>> GetSectionsBySpeakerIdAsync(int id)
        {
            return await _context.SectionsSpeakers
                .Where(ss => ss.SectionId == id)
                .Select(st => st.Section)
                .ToListAsync();
        }

        public async Task<List<Section>> GetSectionsByDayIdAsync(int id)
        {
            return await _context.Sections
                .Where(section => section.DayId == id)
                .ToListAsync();
    
        }

        public async Task<List<Section>> GetSectionsByTagIdAsync(int id)
        {
            return await _context.SectionsTags
                .Where(st => st.TagId == id)
                .Select(st => st.Section)
                .ToListAsync();
        }
    }
}
