using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;

namespace MITSDataLib.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly MITSContext _context;

        public TagsRepository(MITSContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await _context.Tags
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Tag>> GetTagsBySectionIdAsync(int id)
        {
            return await _context.SectionsTags
                .Where(st => st.SectionId == id)
                .Select(st => st.Tag)
                .ToListAsync();
        }
    }
}
