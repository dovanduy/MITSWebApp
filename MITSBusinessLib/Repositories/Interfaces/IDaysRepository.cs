using System.Collections.Generic;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IDaysRepository
    {
        Task<List<Day>> GetDaysAsync();
        Task<Day> CreateDayAsync(Day newDay);
        Task<List<Day>> DeleteDayAsync(int dayId);
    }
}
