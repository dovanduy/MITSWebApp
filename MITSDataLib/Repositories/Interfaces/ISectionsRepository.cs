using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface ISectionsRepository
    {
        Task<List<Section>> GetSectionsByDayIdAsync(int id);
        Task<List<Section>> GetSectionsByTagIdAsync(int id);
        Task<List<Section>> GetSectionsAsync();
    }
}
