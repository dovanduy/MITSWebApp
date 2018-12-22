using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IWaRepository
    {
        Task<WildApricotToken> GetTokenAsync();
        Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken);
    }
}