using MITSDataLib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IOrganizerRepo
    {
        Task<List<Speaker>> GetSpeakers();
        Task<Speaker> GetSpeakers(int id);
        Task<Speaker> InsertSpeaker(Speaker speaker);
        Task<bool> UpdateSpeaker(int id, Speaker speakerWithUpdate);
        Task<bool> DeleteSpeaker(int id);
    }
}
