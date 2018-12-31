using System.Collections.Generic;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IEventsRepository
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<List<Event>> GetEventsAsync();
        Task<Event> CreateEvent(Event newEvent);
        Task<List<WildApricotEvent>> GetWaEventByEventId(int id);
        Task<List<WildApricotRegistrationType>> GetWaRegistrationTypesByWaEventId(int id);
    }
}
