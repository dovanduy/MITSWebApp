using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Models;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.ResponseModels.WildApricot;
using MITSBusinessLib.Utilities;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MITSBusinessLib.Repositories
{
    public class WaRepository : IWaRepository
    {
        private readonly MITSContext _context;
        private readonly string _apiKey;
        private readonly int _accountId = 257051;

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

        public async Task<EventResponse> GetWaEventDetails(int eventId)
        {

            var apiEventResource = $"accounts/{_accountId}/events/{eventId}";

            var response = new HttpResponseMessage();
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.GetRequest(apiEventResource, token);
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

            var eventDetailsResponse = await GetWaEventDetails(eventAddedToDB.MainEventId);

            if (eventDetailsResponse == null)
            {
                throw new ExecutionError("Error Wild Apricot Event could not be found");
            }

            var registrationTypes = eventDetailsResponse.Details.RegistrationTypes;

            var newRegistrationTypes = new List<WildApricotRegistrationType>();


            //EventDetailsResponse should not be null because there is a null check in GetWaEventDetails

            var newWaEvent = new WildApricotEvent
            {
                Description = eventDetailsResponse.Details.DescriptionHtml,
                Name = eventDetailsResponse.Name,
                Location = eventDetailsResponse.Location,
                StartDate = eventDetailsResponse.StartDate,
                EndDate = eventDetailsResponse.EndDate,
                IsEnabled = eventDetailsResponse.RegistrationEnabled,
                RegistrationsLimit = eventDetailsResponse.RegistrationsLimit ?? 0,
                Event = eventAddedToDB,
                WaRegistrationTypes = newRegistrationTypes

            };

            

            registrationTypes.ForEach(rt =>
            {
                var newRegistrationType = new WildApricotRegistrationType
                {
                    RegistrationTypeId = rt.Id,
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

        public async Task<Contact> GetContact(string email)
        {
            var apiEventResource = $"accounts/{_accountId}/contacts";
            var query = new List<string>
            {
                "$async=false",
                $"simpleQuery={email}"
            };

            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.GetRequest(apiEventResource, token, query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<QueryContactResponse>(content);

            if (result.Contacts.Count == 0)
            {
                return null;
            }

            var returnedContact = result.Contacts[0];

            return new Contact
            {
                FirstName = returnedContact.FirstName,
                LastName = returnedContact.LastName,
                Email = returnedContact.Email,
                Organization = returnedContact.Organization,
                Status = returnedContact.Status,
                Id = returnedContact.Id
                
            };
                              
        }

        public async Task<Contact> CreateContact(Registration newRegistration)
        {
            var newContact = new NewContact
            {
                FirstName = newRegistration.FirstName,
                LastName = newRegistration.LastName,
                Email = newRegistration.Email,
                Organization = newRegistration.Organization,
                Status = "Active",
                RecreateInvoice = false
            };

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newContactString = JsonConvert.SerializeObject(newContact, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var encodedContact = new StringContent(newContactString, Encoding.UTF8, "application/json");

            var apiEventResource = $"accounts/{_accountId}/contacts";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, encodedContact);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<NewContactResponse>(content);

            return new Contact
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                Organization = result.Organization,
                Status = "Active",
                Id = result.Id

            };

        }

        public async Task<bool> DeleteEventRegistration(int registrationId)
        {
            var apiEventResource = $"accounts/{_accountId}/eventregistrations/{registrationId}";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<int> AddEventRegistration(Registration newRegistration, int contactId)
        {
            var newRegistrationData = new NewEventRegistration
            {
                Event = new WaEvent
                {
                    Id = newRegistration.EventId
                },
                Contact = new WaContact
                {
                    Id = contactId
                },
                RegistrationTypeId = newRegistration.RegistrationTypeId,
                IsCheckedIn = false,
                RegistrationFields = new List<RegistrationField>
                {
                    new RegistrationField
                    {
                        FieldName = "First name",
                        Value = newRegistration.FirstName,
                        SystemCode = "FirstName"
                    },
                    new RegistrationField
                    {
                        FieldName = "Last name",
                        Value = newRegistration.LastName,
                        SystemCode = "LastName"
                    },
                    new RegistrationField
                    {
                        FieldName = "Title",
                        Value = newRegistration,
                        SystemCode = "Title"
                    },
                    new RegistrationField
                    {
                        FieldName = "e-mail",
                        Value = newRegistration.Email,
                        SystemCode = "Email"
                    },
                    new RegistrationField
                    {
                        FieldName = "Registration Terms and Conditions",
                        Value = true,
                        SystemCode = "custom-10687529"
                    },
                    new RegistrationField
                    {
                        FieldName = "AFCEA Member ID#",
                        Value = newRegistration.MemberId,
                        SystemCode = "custom-10687532"
                    }
                },
                Memo = "Event Registration Created by MITS Web App"

            };

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newRegistrationString = JsonConvert.SerializeObject(newRegistrationData, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var encodedRegistration = new StringContent(newRegistrationString, Encoding.UTF8, "application/json");

            var apiEventResource = $"accounts/{_accountId}/eventregistrations";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, encodedRegistration);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EventRegistrationResponse>(content);

            return result.Id;
        }

        public async Task<int> AddSponsorRegistration(Sponsor newSponsor, int contactId)
        {
            var newRegistrationData = new NewEventRegistration
            {
                Event = new WaEvent
                {
                    Id = newSponsor.EventId
                },
                Contact = new WaContact
                {
                    Id = contactId
                },
                RegistrationTypeId = newSponsor.RegistrationTypeId,
                IsCheckedIn = false,
                RegistrationFields = new List<RegistrationField>
                {
                    new RegistrationField
                    {
                        FieldName = "First name",
                        Value = newSponsor.FirstName,
                        SystemCode = "FirstName"
                    },
                    new RegistrationField
                    {
                        FieldName = "Last name",
                        Value = newSponsor.LastName,
                        SystemCode = "LastName"
                    },
                    new RegistrationField
                    {
                        FieldName = "Title",
                        Value = newSponsor,
                        SystemCode = "Title"
                    },
                    new RegistrationField
                    {
                        FieldName = "e-mail",
                        Value = newSponsor.Email,
                        SystemCode = "Email"
                    },
                    new RegistrationField
                    {
                        FieldName = "Registration Terms and Conditions",
                        Value = true,
                        SystemCode = "custom-10687529"
                    }
                },
                Memo = "Event Registration Created by MITS Web App"

            };

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newRegistrationString = JsonConvert.SerializeObject(newRegistrationData, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var encodedRegistration = new StringContent(newRegistrationString, Encoding.UTF8, "application/json");

            var apiEventResource = $"accounts/{_accountId}/eventregistrations";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, encodedRegistration);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EventRegistrationResponse>(content);

            return result.Id;
        }

        public async Task<int> GenerateEventRegistrationInvoice(int eventRegistrationId)
        {
            var apiEventResource = $"rpc/{_accountId}/GenerateInvoiceForEventRegistration";

            var query = new List<string>
            {
                $"eventRegistrationId={eventRegistrationId}",
                "updateIfexists=false"
            };

            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, null, query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GenerateEventRegistrationInvoiceResponse>(content);

            return result.Id;
        }

        public async Task<int> MarkInvoiceAsPaid(WildApricotRegistrationType registrationType, int invoiceId, int contactId)
        {
            var newInvoice = new NewInvoice
            {
               Value = registrationType.BasePrice,
                Invoices = new List<Invoice>
                {
                    new Invoice
                    {
                        Id = invoiceId
                    }
                },
                Contact = new InvoiceContact
                {
                    Id = contactId
                },
                Comment = "Marked paid by the MITS Web application",
                PaymentType = "InvoicePayment"
            };

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newInvoiceString = JsonConvert.SerializeObject(newInvoice, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var encodedContact = new StringContent(newInvoiceString, Encoding.UTF8, "application/json");

            var apiEventResource = $"accounts/{_accountId}/payments";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, encodedContact);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<NewInvoiceResponse>(content);

            return result.Id;
        }

        public async Task<EventRegistrationResponse> GetEventRegistration(int registrationId)
        {
            var apiEventResource = $"accounts/{_accountId}/eventregistrations/{registrationId}";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.GetRequest(apiEventResource, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EventRegistrationResponse>(content);

            return result;
        }

        public async Task<bool> CheckInEventAttendee(int registrationId)
        {
            var newEventRegistrationCheckIn = new EventRegistrationCheckIn
            {
                RegistrationId = registrationId,
                CheckedIn = true
            };

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newEventRegistrationCheckInString = JsonConvert.SerializeObject(newEventRegistrationCheckIn, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var encodedContact = new StringContent(newEventRegistrationCheckInString, Encoding.UTF8, "application/json");

            var apiEventResource = $"rpc/{_accountId}/CheckInEventAttendee";
            HttpResponseMessage response;
            var token = await GetTokenAsync();

            try
            {
                response = await WildApricotOps.PostRequest(apiEventResource, token, encodedContact);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = content.Contains("true");

            return result;
        }



        //Maybe I can batch some requests together...http://gethelp.wildapricot.com/en/articles/488-batch-api-requests


        //generate an invoice for the paid member.....https://api.wildapricot.org/v2/rpc/12615/GenerateInvoiceForEventRegistration?eventRegistrationId=24247466&updateIfexists=false 
        //Mark it as paid....https://api.wildapricot.org/v2/accounts/12615/payments 

        //{
        //    "Value": 425,
        //    "Invoices": [
        //    {
        //        "Id": 46428947,
        //        "Url": "string"
        //    }
        //    ],
        //    "Contact": {
        //        "Id": 48952441,
        //        "Url": "string"
        //    },

        //    "Comment": "This was paid through the MITS web app",
        //    "PublicComment": "string",
        //    "PaymentType": "InvoicePayment"
        //}


        //Used for checking in event attendee....https://api.wildapricot.org/v2/rpc/12615/CheckInEventAttendee

        //{
        //  "RegistrationId": 0,
        //  "CheckedIn": true,
        //  "CheckedInGuests": 0
        //}



        //Must create new contact before event registration
        //Must first query for contact by unique email and then if there isn't one add the new contact

        //Get a contact...https://api.wildapricot.org/v2/accounts/12615/contacts?simpleQuery=brandy.canty@gmail.com 
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