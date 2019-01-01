﻿using System;
using System.Threading.Tasks;
using MITSBusinessLib.Repositories.Interfaces;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly MITSContext _context;

        public RegistrationRepository(MITSContext context)
        {
            _context = context;
        }

        public async Task<EventRegistrationAudit> CreateEventRegistrationAudit(Registration newEventRegistration)
        {
            var newRegistrationAudit = new EventRegistrationAudit
            {
                EventId = newEventRegistration.EventId.ToString(),
                RegistrationTypeId = newEventRegistration.RegistrationTypeId.ToString(),
                FirstName = newEventRegistration.FirstName,
                LastName = newEventRegistration.LastName,
                Email = newEventRegistration.Email,
                Modified = DateTime.Now,
                Status = "Starting"

            };

            await _context.AddAsync(newRegistrationAudit);
            await _context.SaveChangesAsync();
            return newRegistrationAudit;
        }

        public async Task<EventRegistrationAudit> UpdateEventRegistrationAudit(
            EventRegistrationAudit eventRegistrationAudit, string status)
        {
            eventRegistrationAudit.Status = status;

            await _context.SaveChangesAsync();

            return eventRegistrationAudit;
        }
    }
}
