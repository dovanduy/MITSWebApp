using Microsoft.Extensions.Logging;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MITSBusinessLib.Repositories
{
   public  class EventOrganizerRepo : IEventOrganizerRepo
    {
        private readonly MITSContext _ctx;
        private readonly ILogger<EventOrganizerRepo> _logger;

        public EventOrganizerRepo(MITSContext context, ILogger<EventOrganizerRepo> logger)
        {
           
            _ctx = context;
            _logger = logger;
        }

        #region speaker

        public async Task<bool> DeleteSpeaker(int id)
        {
            var speakerToDelete = await _ctx.Speakers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(speaker => speaker.Id == id);

            try
            {

                if (speakerToDelete == null)
                {
                    return false;
                }
                else
                {
                    _ctx.Speakers.Remove(speakerToDelete);
                    await _ctx.SaveChangesAsync();
                    return true;
                }
            }

            catch (System.Exception exp)
            {
                _logger.LogError($"Error in {nameof(DeleteSpeaker)}: " + exp.Message);
            }
            return false;
        }

        public async Task<List<Speaker>> GetSpeakers()
        {
            return await _ctx.Speakers
                .OrderBy(speaker => speaker.LastName)
                .ToListAsync();
        }

        public async Task<Speaker> GetSpeakers(int id)
        {
            return await _ctx.Speakers
                .AsNoTracking()
                .SingleOrDefaultAsync(speaker => speaker.Id == id);
                     
        }

        public async Task<Speaker> InsertSpeaker(Speaker speaker)
        {
            _ctx.Add(speaker);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(InsertSpeaker)}: " + exp.Message);
            }

            return speaker;
        }

        public async Task<bool> UpdateSpeaker(int id, Speaker speakerWithUpdate)
        {

            var speakerToUpdate = await _ctx.Speakers
                        .AsNoTracking()
                        .SingleOrDefaultAsync(speaker => speaker.Id == id);

            try
            {
                
                if (speakerToUpdate == null || id != speakerWithUpdate.Id)
                {
                    return false;
                }
                else
                {
                    _ctx.Update(speakerWithUpdate);
                    await _ctx.SaveChangesAsync();
                    return true;

                }
            }
            catch (Exception exp)
            {
                _logger.LogError($"Error in {nameof(UpdateSpeaker)}: " + exp.Message);
            }
            return false;
        }
 
    }
    #endregion
}
