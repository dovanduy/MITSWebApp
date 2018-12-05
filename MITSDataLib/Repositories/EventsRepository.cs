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
        private readonly MITSContext context;

        public EventsRepository(MITSContext context)
        {
            this.context = context;
        }

        public async Task<Event> GetEvent(int id)
        {
           return await context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        
        public async Task<List<Event>> GetEvents()
        {
            return await context.Events
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Event> AddEvent(Event newEvent)
        {
            await context.AddAsync(newEvent);
            await context.SaveChangesAsync();
            return newEvent;
        }

      
    }
}
