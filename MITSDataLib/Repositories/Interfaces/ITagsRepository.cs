using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetTagsAsync();
        Task<List<Tag>> GetTagsBySectionIdAsync(int id);
    }
}
