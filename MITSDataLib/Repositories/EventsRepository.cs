using MITSDataLib.Contexts;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MITSDataLib.Models;
using System.Threading.Tasks;

namespace MITSDataLib.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly MITSContext _context;

        public EventsRepository(MITSContext context)
        {
            this._context = context;
        }

        public async Task<Event> GetEvent(int id)
        {
           return await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        
        public async Task<List<Event>> GetEvents()
        {
            return await _context.Events
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Event> CreateEvent(Event newEvent)
        {
            await _context.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<List<WildApricotEvent>> GetWaEventByEventId(int id)
        {
            return await _context.WaEvents
                .Where(waEvent => waEvent.EventId == id)
                .ToListAsync();
        }

        public async Task<List<WildApricotRegistration>> GetWaRegistrationTypesByWaEventId(int id)
        {
            return await _context.WaRegistrations
                .Where(war => war.WaEventId == id)
                .ToListAsync();
        }
    }
}
