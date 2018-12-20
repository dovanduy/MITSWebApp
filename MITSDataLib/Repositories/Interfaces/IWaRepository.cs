using MITSDataLib.Models;
using System.Threading.Tasks;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface IWaRepository
    {
        Task<WildApricotToken> GetTokenAsync();
        Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken);
    }
}