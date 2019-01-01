﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MITSBusinessLib.Models;
using MITSBusinessLib.ResponseModels.WildApricot;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IWaRepository
    {
        Task<WildApricotToken> GetTokenAsync();
        Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken, bool updateToken);
        Task<EventResponse> GetWaEventDetails(Event newEvent);
        Task<Event> AddWildApricotEvent(Event newEvent);
        Task<Contact> GetContact(string email);
        Task<Contact> CreateContact(Registration newRegistration);
        Task<int> AddEventRegistration(Registration newRegistration, int contactId);
        Task<int> GenerateEventRegistrationInvoice(int eventRegistrationId);
        Task<int> MarkInvoiceAsPaid(WildApricotRegistrationType registrationType, int invoiceId, int contactId);
    }
}