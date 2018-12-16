using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL;

namespace MITSDataLib.Repositories
{
    public class DaysRepository : IDaysRepository
    {
        private readonly MITSContext _context;

        public DaysRepository(MITSContext context)
        {
            _context = context;
        }

        public async Task<Day> CreateDayAsync (Day newDay)
        {
            await _context.AddAsync(newDay);
            await _context.SaveChangesAsync();
            return newDay;
        }

        public async Task<List<Day>> DeleteDayAsync(int dayId)
        {

            var dayToDelete = await _context.Days
                .AsNoTracking()
                .SingleOrDefaultAsync(day => day.Id == dayId);
            
            if (dayToDelete == null)
            {
                throw new ExecutionError("Day could not be found");             
            }

            try
            {
                _context.Days.Remove(dayToDelete);
                await _context.SaveChangesAsync();
                return await GetDaysAsync();

            }
            catch (Exception e)
            {
                throw new ExecutionError($"There was an error deleting {dayId}: {e.Message}");
            }

        }

        public async Task<List<Day>> GetDaysAsync()
        {
            var days = await _context.Days
                .AsNoTracking()
                .ToListAsync();

            return days;
        }
    }
}
