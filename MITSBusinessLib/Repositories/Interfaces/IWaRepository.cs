using System.Collections.Generic;
using System.Threading.Tasks;
using MITSBusinessLib.Models;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IWaRepository
    {
        Task<WildApricotToken> GetTokenAsync();
        Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken, bool updateToken);
        Task<EventResponse> GetWaEventDetails(Event newEvent);
        Task<Event> AddWildApricotEvent(Event newEvent);
    }
}