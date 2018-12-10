using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface IDaysRepository
    {
        Task<List<Day>> GetDaysAsync();
        Task<Day> CreateDayAsync(Day newDay);
        Task<List<Day>> DeleteDayAsync(int dayId);
    }
}
