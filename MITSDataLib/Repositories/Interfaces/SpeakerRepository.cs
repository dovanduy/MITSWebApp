using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly MITSContext context;

        public SpeakerRepository(MITSContext context)
        {
            this.context = context;
        }

        public async Task<List<Speaker>> GetSpeakersAsync()
        {
            return await context.Speakers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Speaker>> GetSpeakersBySectionIdAsync(int id)
        {
            return await context.SectionsSpeakers
                .Where(ss => ss.SectionId == id)
                .Select(ss => ss.Speaker)
                .ToListAsync();
                
                
        }
    }
}
