using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Models;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.Utilities;
using MITSDataLib.Contexts;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories
{
    public class WaRepository : IWaRepository
    {
        private readonly MITSContext _context;
        private readonly string _apiKey;
        private readonly int _accountId = 12615;

        public WaRepository(MITSContext context, IConfiguration config)
        {
            _context = context;
            _apiKey = config.GetSection("Secrets:APIKEY").Value;
        }

        public async Task<WildApricotToken> GetTokenAsync()
        {
            var updateToken = false;
            var token = await  _context.WaTokens.SingleOrDefaultAsync();


            if (token != null)
            {
                var tokenExpires = token?.TokenExpires ?? DateTime.Now;
                if (DateTime.Compare(tokenExpires, DateTime.Now.AddMinutes(3)) < 0)
                {
                    return token;
                }

                updateToken = true;
            }


            var tokenResponse = await WildApricotOps.GenerateNewAccessToken(_apiKey);

            var newToken = new WildApricotToken
            {
                AccessToken = tokenResponse.access_token,
                TokenExpires = DateTime.Now.AddSeconds(tokenResponse.expires_in)
            };

            return await SetTokenAsync(newToken, updateToken);

        }

        public async Task<WildApricotToken> SetTokenAsync(WildApricotToken respToken, bool updateToken)
        {
            
            if (updateToken)
            {

                respToken.AccessToken = respToken.AccessToken;
                respToken.TokenExpires = respToken.TokenExpires;
                await _context.SaveChangesAsync();
                
            }
            else
            {
                try
                {
                    _context.WaTokens.Add(respToken);

                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            
            return respToken;

        }

        public async Task<EventResponse> GetWaEventDetails(Event eventAddedToDB)
        {

            var apiEventResource = $"accounts/{_accountId}/events/{eventAddedToDB.MainEventId}";

            var response = new HttpResponseMessage();
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.GetResponse(apiEventResource, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var apiResponse = await response.Content.ReadAsStringAsync();

            var eventDetailsReturned = Newtonsoft.Json.JsonConvert.DeserializeObject<EventResponse>(apiResponse);

            if (eventDetailsReturned == null)
            {
                throw new ExecutionError("Event could not be found");
            }

            return eventDetailsReturned;

        }

        public async Task<Event> AddWildApricotEvent(Event eventAddedToDB)
        {

            var eventDetailsResponse = await GetWaEventDetails(eventAddedToDB);

            if (eventDetailsResponse == null)
            {
                throw new ExecutionError("Error Wild Apricot Event could not be found");
            }

            var registrationTypes = eventDetailsResponse.Details.RegistrationTypes;

            var newRegistrationTypes = new List<WildApricotRegistration>();


            //EventDetailsResponse should not be null because there is a null check in GetWaEventDetails

            var newWaEvent = new WildApricotEvent
            {
                Description = eventDetailsResponse.Details.DescriptionHtml,
                Name = eventDetailsResponse.Name,
                Location = eventDetailsResponse.Location,
                StartDate = eventDetailsResponse.StartDate,
                EndDate = eventDetailsResponse.EndDate,
                IsEnabled = eventDetailsResponse.RegistrationEnabled,
                Event = eventAddedToDB,
                WaRegistrationTypes = newRegistrationTypes

            };

            

            registrationTypes.ForEach(rt =>
            {
                var newRegistrationType = new WildApricotRegistration
                {
                    IsEnabled = rt.IsEnabled,
                    Description = rt.Description,
                    BasePrice = rt.BasePrice,
                    CodeRequired = (rt.Availability.Contains("CodeRequired")),
                    RegistrationCode = rt.RegistrationCode,
                    AvailableFrom = rt.AvailableFrom,
                    AvailableThrough = rt.AvailableThrough,
                    Name = rt.Name,
                    WaEvent = newWaEvent


                };

                newRegistrationTypes.Add(newRegistrationType);
            });


            

            
            await _context.WaEvents.AddAsync(newWaEvent);
            await _context.WaRegistrations.AddRangeAsync(newRegistrationTypes);
            await _context.SaveChangesAsync();

            return eventAddedToDB;
        }

        //setToken

        //replaceToken

        //deleteToken
    }
}