using System.Collections.Generic;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface ISectionsRepository
    {
        Task<List<Section>> GetSectionsByDayIdAsync(int id);
        Task<List<Section>> GetSectionsByTagIdAsync(int id);
        Task<List<Section>> GetSectionsAsync();
        Task<List<Section>> GetSectionsBySpeakerIdAsync(int id);
    }
}
