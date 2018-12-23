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
                if (DateTime.Compare(tokenExpires, DateTime.Now.AddMinutes(3)) > 0)
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

        //Need to figure out how to generate an invoice for the paid member and mark it as paid. 

        //Used for checking in event attendee....https://api.wildapricot.org/v2/rpc/12615/CheckInEventAttendee

        //Must create new contact before event registration
        //Must first query for contact by unique email and then if there isn't one add the new contact

        //Retrieve a contact...https://api.wildapricot.org/v2/accounts/12615/contacts?simpleQuery=brandy.canty@gmail.com 
        //Get the result if it is complete....https://api.wildapricot.org/v2/accounts/12615/contacts?resultId=2fb32e1b-869e-4db4-8c17-3c80c148e76e

        //Create new contact....https://api.wildapricot.org/v2/accounts/12615/contacts
        //{
        //    "FirstName": "Bob",
        //    "LastName": "Anderson",
        //    "Organization": "Pinhest Corp",
        //    "Email": "bob@bob.com",
        //    "Status": "Active",
        //    "RecreateInvoice": true
        //}

        //Create new Event registration JSON body...https://api.wildapricot.org/v2/accounts/12615/eventregistrations


        //  "Event": {
        //    "Id": 3176755
        //  },
        //  "Contact": {
        //    "Id": 48952441
        //  },
        //  "RegistrationTypeId": 4574357,
        //  "IsCheckedIn": false,
        //  "RegistrationFields": [
        //		{
        //	        "FieldName": "First name",
        //	        "Value": "Bob",
        //	        "SystemCode": "FirstName"

        //        },
        //	    {
        //	        "FieldName": "Last name",
        //	        "Value": "Anderson",
        //	        "SystemCode": "LastName"
        //	    },
        //	    {
        //	        "FieldName": "Organization",
        //	        "Value": "",
        //	        "SystemCode": "Organization"
        //	    },
        //	    {
        //	        "FieldName": "e-Mail",
        //	        "Value": "bob.Anderson@gmail.com",
        //	        "SystemCode": "Email"
        //	    },
        //	    {
        //	        "FieldName": "Phone",
        //	        "Value": "3344671140",
        //	        "SystemCode": "Phone"
        //	    },
        //	    {
        //	        "FieldName": "Cell Phone",
        //	        "Value": "",
        //	        "SystemCode": "custom-2646430"
        //	    },
        //	    {
        //	        "FieldName": "Title",
        //	        "Value": "",
        //	        "SystemCode": "custom-2646431"
        //	    },
        //	    {
        //	        "FieldName": "Registration Terms and Conditions",
        //	        "Value": true,
        //	        "SystemCode": "custom-10687529"
        //	    },
        //	    {
        //	        "FieldName": "AFCEA Member ID#",
        //	        "Value": "342343",
        //	        "SystemCode": "custom-10687532"
        //	    },
        //	    {
        //	        "FieldName": "AFCEA Membership Expire Date",
        //	        "Value": "2019-04-18T19:00:00-05:00",
        //	        "SystemCode": "custom-10687533"
        //	    },
        //	    {
        //	        "FieldName": "AFCEA Life Member",
        //	        "Value": [
        //	            {
        //	                "Id": 11477352,
        //	                "Label": "Yes"
        //	            }
        //	        ],
        //	        "SystemCode": "custom-10687534"
        //	    },
        //	    {
        //	        "FieldName": "Local or Traveling",
        //	        "Value": {
        //	            "Id": 11477353,
        //	            "Label": "Local Montgomery Area"
        //	        },
        //	        "SystemCode": "custom-10687535"
        //	    }

        //  ],
        //  "ShowToPublic": false,
        //  "RegistrationDate": "2018-12-23T19:00:00-05:00",
        //  "Memo": "This was automatically loaded by MITS Conference App",
        //  "RecreateInvoice": false


       
    }
}