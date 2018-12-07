using Microsoft.EntityFrameworkCore;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MITSDataLib.Repositories
{
    public class DaysRepository : IDaysRepository
    {
        private readonly MITSContext context;

        public DaysRepository(MITSContext context)
        {
            this.context = context;
        }

        public async Task<List<Day>> GetDays()
        {
            return await context.Days
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
