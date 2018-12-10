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
    public class SpeakersRepository : ISpeakersRepository
    {
        private readonly MITSContext _context;

        public SpeakersRepository(MITSContext context)
        {
            this._context = context;
        }

        public async Task<List<Speaker>> GetSpeakersAsync()
        {
            var speakers = await _context.Speakers
                .AsNoTracking()
                .ToListAsync();

            return speakers;
        }

        public async Task<List<Speaker>> GetSpeakersBySectionIdAsync(int id)
        {
            return await _context.SectionsSpeakers
                .Where(ss => ss.SectionId == id)
                .Select(ss => ss.Speaker)
                .ToListAsync();
                
                
        }

        public async Task<Speaker> GetSpeakerByIdAsync(int speakerId)
        {
            return await _context.Speakers
                .AsNoTracking()
                .FirstOrDefaultAsync(speaker => speaker.Id == speakerId);

           
        }
    }
}
