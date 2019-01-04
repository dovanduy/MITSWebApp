using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MITSBusinessLib.ResponseModels.WildApricot;
using MITSDataLib.Models;

namespace MITSBusinessLib.Business
{
    public interface IEventRegistrationBusinessLogic
    {
        Task<Registration> RegisterAttendee(Registration newRegistration);
        Task<CheckInAttendee> CheckInAttendee(CheckInAttendee attendee);
    }
}
