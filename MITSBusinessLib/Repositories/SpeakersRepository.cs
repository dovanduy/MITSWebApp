using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories
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

        public async Task<Speaker> CreateSpeakerAsync(Speaker newSpeaker)
        {
            await _context.AddAsync(newSpeaker);
            await _context.SaveChangesAsync();
            return newSpeaker;
        }

        public async Task<Speaker> UpdateSpeakerAsync(Speaker newSpeakerValues)
        {
            var speakerToUpdate = await _context.Speakers
                .SingleOrDefaultAsync(speaker => speaker.Id == newSpeakerValues.Id);

            if (speakerToUpdate == null)
            {
                throw new ExecutionError("User could not be found");
            }

            speakerToUpdate.FirstName = newSpeakerValues.FirstName;
            speakerToUpdate.LastName = newSpeakerValues.LastName;
            speakerToUpdate.Title = newSpeakerValues.Title;
            speakerToUpdate.Bio = newSpeakerValues.Bio;

            await _context.SaveChangesAsync();

            return speakerToUpdate;


        }

        public async Task<int> DeleteSpeakerAsync(int speakerId)
        {
            var speakerToDelete = await _context.Speakers
                .AsNoTracking()
                .SingleOrDefaultAsync(speaker => speaker.Id == speakerId);

            if (speakerToDelete == null)
            {
                throw new ExecutionError("Speaker could not be found");
            }

            try
            {
                _context.Speakers.Remove(speakerToDelete);
                var numberRecordsUpdated = await _context.SaveChangesAsync();
                return numberRecordsUpdated;

            }
            catch (Exception e)
            {
                throw new ExecutionError($"There was an error deleting {speakerId}: {e.Message}");
            }
        }
    }
}
