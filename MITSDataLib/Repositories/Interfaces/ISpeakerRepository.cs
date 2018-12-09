using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSDataLib.Repositories.Interfaces
{
    public interface ISpeakerRepository
    {
        Task<List<Speaker>> GetSpeakersAsync();
        Task<List<Speaker>> GetSpeakersBySectionIdAsync(int id);
    }
}
