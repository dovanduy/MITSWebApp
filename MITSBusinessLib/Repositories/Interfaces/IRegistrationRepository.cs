using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<EventRegistrationAudit> CreateEventRegistrationAudit(Registration newEventRegistrationAudit);
        Task<EventRegistrationAudit> UpdateEventRegistrationAudit(EventRegistrationAudit eventRegistrationAudit,
            string status);
        Task<EventCheckInAudit> CreateEventCheckinAudit(EventCheckInAudit checkInAudit)
    }
}
