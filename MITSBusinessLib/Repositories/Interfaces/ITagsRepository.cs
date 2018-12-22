using System.Collections.Generic;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetTagsAsync();
        Task<List<Tag>> GetTagsBySectionIdAsync(int id);
    }
}
