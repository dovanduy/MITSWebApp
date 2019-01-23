using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Conversion;
using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.ResponseModels.Authorize;
using MITSBusinessLib.ResponseModels.WildApricot;
using MITSBusinessLib.Utilities;
using MITSDataLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MITSBusinessLib.Business
{
    public class EventRegistrationBusinessLogic : IEventRegistrationBusinessLogic
    {

        private readonly IWaRepository _waRepo;
        private readonly IEventsRepository _eventsRepository;
        private readonly IAuditRepository _registrationRepo;
        private readonly IMailOps _mailOps;
        private readonly string _name;
        private readonly string _transactionKey;

        public EventRegistrationBusinessLogic(IWaRepository waRepo, IEventsRepository eventsRepo, IAuditRepository registrationRepo, IConfiguration config, IMailOps mailOps)
        {
            _waRepo = waRepo;
            _eventsRepository = eventsRepo;
            _registrationRepo = registrationRepo;
            _mailOps = mailOps;
            _name = config.GetSection("Secrets:Name").Value;
            _transactionKey = config.GetSection("Secrets:TransactionKey").Value;
        }

        public async Task<Registration> RegisterAttendee(Registration newRegistration)
        {
            var eventRegistrationAudit = await _registrationRepo.CreateEventRegistrationAudit(newRegistration);

            if (eventRegistrationAudit == null)
            {
                Console.WriteLine("Audit was not created");
            }

            bool isPaidEvent = !string.IsNullOrEmpty(newRegistration.DataDescriptor) &&
                               !string.IsNullOrEmpty(newRegistration.DataValue);

            var registrationTypeDetails =
                await _eventsRepository.GetEventTypeById(newRegistration.RegistrationTypeId);

            if (!newRegistration.RegistrationCode.IsEmpty())
            {
                if (newRegistration.RegistrationCode != registrationTypeDetails.RegistrationCode)
                {
                    throw new ExecutionError("Invalid Registration Code");
                }
            }

            //Retrieve Contact from WildApricot
            //Create Contact if needed
            var contact = await _waRepo.GetContact(newRegistration.Email) ?? await _waRepo.CreateContact(newRegistration);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Contact Created - {contact.Id}");
            

            //Create Event Registration

            var eventRegistrationId = await _waRepo.AddEventRegistration(newRegistration, contact.Id);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Event Registration Created - {eventRegistrationId}");

            if (isPaidEvent)
            {
                //Create Invoice

                var invoiceId = await _waRepo.GenerateEventRegistrationInvoice(eventRegistrationId);
                await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                    $"Event Invoice Created - {invoiceId}");

                //Process Payment... What happens if the payment fails? Will the user be able to register again? What happens to their other event registration?
            
                var processTransaction = new ProcessTransaction
                {
                    CreateTransactionRequest = new CreateTransactionRequest
                    {
                        MerchantAuthentication = new MerchantAuthentication
                        {
                            Name = _name,
                            TransactionKey = _transactionKey
                        },
                        TransactionRequest = new TransactionRequest
                        {
                            TransactionType = "authCaptureTransaction",
                            Amount = registrationTypeDetails.BasePrice.ToString(CultureInfo.InvariantCulture),
                            Payment = new Payment
                            {
                                OpaqueData = new OpaqueData
                                {
                                    DataDescriptor = newRegistration.DataDescriptor,
                                    DataValue = newRegistration.DataValue
                                }
                            },
                            Order = new Order
                            {
                                InvoiceNumber = $"{invoiceId}",
                                Description = $"Registration for {registrationTypeDetails.Name}"

                            },
                            LineItems = new LineItems
                            {
                                LineItem = new LineItem
                                {
                                    ItemId = $"{invoiceId}",
                                    Name = $"Invoice #{invoiceId}",
                                    Description = $"Registration for {registrationTypeDetails.Name}",
                                    Quantity = "1",
                                    UnitPrice = registrationTypeDetails.BasePrice.ToString(CultureInfo.InvariantCulture)
                                }
                            }
                            //Use if we have Address Authentication turned on
                            //,
                            //BillTo = new BillTo
                            //{
                            //    FirstName = "Bob",
                            //    LastName = "Anderson",
                            //    Company = "Bob Anderson",
                            //    Address = "35 Testing Rd.",
                            //    City = "Montgomery",
                            //    Country = "USA",
                            //    State = "AL",
                            //    //Can be used to generate errors in testing
                            //    Zip = "46203"

                            //}
                        }
                    }
                };

                var transactionResponse = await AuthorizeOps.CreateTransaction(processTransaction);

                var transactionResponseContent = await transactionResponse.Content.ReadAsStringAsync();

                var transactionResponseResult =
                    Newtonsoft.Json.JsonConvert
                        .DeserializeObject<CreateTransactionResponse>(transactionResponseContent);

                //handle all the errors in the transactionResponseResult

                if (transactionResponseResult != null)
                {

                    if (transactionResponseResult.Messages.ResultCode == "Ok")
                    {
                        if (transactionResponseResult.TransactionResponse.Messages != null)
                        {
                            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                                "Event Payment Processed");
                        }
                        else
                        {
                           
                            if (transactionResponseResult.TransactionResponse.Errors != null)
                            {

                                await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                                    "Event Payment Failed " + transactionResponseResult.TransactionResponse.Errors[0].ErrorCode);

                                if (!await _waRepo.DeleteEventRegistration(eventRegistrationId))
                                {
                                    throw new ExecutionError(
                                        "Transaction Failed and event Registration Cleanup Failed. Please contact our Help Desk to complete your registration");
                                }

                                throw new ExecutionError(transactionResponseResult.TransactionResponse.Errors[0].ErrorText);
                            }

                            
                        }
                    }
                    else
                    {
                        await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                            "Event Payment Failed " + transactionResponseResult.Messages.Message[0].Code);

                        if (!await _waRepo.DeleteEventRegistration(eventRegistrationId))
                        {
                            throw new ExecutionError(
                                "Transaction Failed and event Registration Cleanup Failed. Please contact our Help Desk to complete your registration");
                        }

                        if (transactionResponseResult.TransactionResponse != null && transactionResponseResult.TransactionResponse.Errors != null)
                        {
                            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                                "Event Payment Failed " + transactionResponseResult.TransactionResponse.Errors[0].ErrorCode);
                            throw new ExecutionError(transactionResponseResult.TransactionResponse.Errors[0].ErrorText);
                        }

                        
                        throw new ExecutionError(transactionResponseResult.Messages.Message[0].Text);
                    }
                }

                //Create payment for invoice
                var paymentId = await _waRepo.MarkInvoiceAsPaid(registrationTypeDetails, invoiceId, contact.Id);
                await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                    $"Invoice Marked as Paid - {paymentId}");
            }


            //Generate HTML Ticket/QR Code and store on server in WWWRoot           
            var registrantGuid = TicketOps.GenerateTicket(eventRegistrationId);

            //Send email with Confirmation and QR code
            _mailOps.Send(newRegistration.Email, eventRegistrationId, registrantGuid);

            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Registration Complete - {eventRegistrationId}");

            //Return Event Registration Id and QR Code Bitmap
            return new Registration()
            {
                EventRegistrationId = eventRegistrationId,
                QrCode = registrantGuid
            };
        }

        public async Task<CheckInAttendee> CheckInAttendee(CheckInAttendee attendee)
        {
            //Check if attendee is registered

            var result = await _waRepo.GetEventRegistration(attendee.RegistrationId);

            var newCheckInAttendee = new CheckInAttendee
            {
                RegistrationId = attendee.RegistrationId,
            };

            if (result == null)
            {
                throw new ExecutionError("Registration could not be found");
            }

            

            if (result.IsPaid)
            {
                
                //Returns True if member was successfully checked in
                if (await _waRepo.CheckInEventAttendee(attendee.RegistrationId))
                {
                    newCheckInAttendee.CheckedIn = true;
                    newCheckInAttendee.Status = "Member is Checked In";
                    return newCheckInAttendee;
                }

                newCheckInAttendee.CheckedIn = false;
                newCheckInAttendee.Status = "Member has paid but there was a problem checking them in";

            }

            newCheckInAttendee.CheckedIn = false;
            newCheckInAttendee.Status = "Member was not check in. Member has an outstanding balance";

            return newCheckInAttendee;
        }

        public async Task<Sponsor> RegisterSponsor(Sponsor newSponsorRegistration)
        {
            var eventRegistrationAudit = await _registrationRepo.CreateEventRegistrationAudit(newSponsorRegistration);

            if (eventRegistrationAudit == null)
            {
                Console.WriteLine("Audit was not created");
            }

            var registrationTypeDetails =
                await _eventsRepository.GetEventTypeById(newSponsorRegistration.RegistrationTypeId);

            //Check if sponsor registration should be disabled
            var eventDetails = await _waRepo.GetWaEventDetails(newSponsorRegistration.EventId);

            var confirmedRegistrationCount = eventDetails.ConfirmedRegistrationsCount;
            var registrationLimit = eventDetails.RegistrationsLimit ?? 0;

            if (registrationLimit > 0)
            {
                if (confirmedRegistrationCount >= registrationLimit)
                {
                    //Mark event as disabled

                    await _eventsRepository.MarkEventAsDisabled(newSponsorRegistration.EventId);
                    throw new ExecutionError("The registration limit for this event has been reached.");
                }

            }

            //Retrieve Contact from WildApricot
            //Create Contact if needed
            var contact = await _waRepo.GetContact(newSponsorRegistration.Email) ?? await _waRepo.CreateContact(new Registration
            {
                FirstName = newSponsorRegistration.FirstName,
                LastName = newSponsorRegistration.LastName,
                Email = newSponsorRegistration.Email,
                Organization = newSponsorRegistration.Organization

            });
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Contact Created - {contact.Id}");

            //Create Event Registration

            var eventRegistrationId = await _waRepo.AddSponsorRegistration(newSponsorRegistration, contact.Id);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Event Registration Created - {eventRegistrationId}");

            //Create Invoice

            var invoiceId = await _waRepo.GenerateEventRegistrationInvoice(eventRegistrationId);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                $"Event Invoice Created - {invoiceId}");

            //Process Payment... What happens if the payment fails? Will the user be able to register again? What happens to their other event registration?

            var processTransaction = new ProcessTransaction
            {
                CreateTransactionRequest = new CreateTransactionRequest
                {
                    MerchantAuthentication = new MerchantAuthentication
                    {
                        Name = _name,
                        TransactionKey = _transactionKey
                    },
                    TransactionRequest = new TransactionRequest
                    {
                        TransactionType = "authCaptureTransaction",
                        Amount = registrationTypeDetails.BasePrice.ToString(CultureInfo.InvariantCulture),
                        Payment = new Payment
                        {
                            OpaqueData = new OpaqueData
                            {
                                DataDescriptor = newSponsorRegistration.DataDescriptor,
                                DataValue = newSponsorRegistration.DataValue
                            }
                        },
                        Order = new Order
                        {
                            InvoiceNumber = $"{invoiceId}",
                            Description = $"Registration for {registrationTypeDetails.Name}"

                        },
                        LineItems = new LineItems
                        {
                            LineItem = new LineItem
                            {
                                ItemId = $"{invoiceId}",
                                Name = $"Invoice #{invoiceId}",
                                Description = $"Registration for {registrationTypeDetails.Name}",
                                Quantity = "1",
                                UnitPrice = registrationTypeDetails.BasePrice.ToString(CultureInfo.InvariantCulture)
                            }
                        }
                        //Use if we have Address Authentication turned on
                        //,
                        //BillTo = new BillTo
                        //{
                        //    FirstName = "Bob",
                        //    LastName = "Anderson",
                        //    Company = "Bob Anderson",
                        //    Address = "35 Testing Rd.",
                        //    City = "Montgomery",
                        //    Country = "USA",
                        //    State = "AL",
                        //    //Can be used to generate errors in testing
                        //    Zip = "46203"

                        //}
                    }
                }
            };

            var transactionResponse = await AuthorizeOps.CreateTransaction(processTransaction);

            var transactionResponseContent = await transactionResponse.Content.ReadAsStringAsync();

            var transactionResponseResult =
                Newtonsoft.Json.JsonConvert
                    .DeserializeObject<CreateTransactionResponse>(transactionResponseContent);

            //handle all the errors in the transactionResponseResult

            if (transactionResponseResult != null)
            {

                if (transactionResponseResult.Messages.ResultCode == "Ok")
                {
                    if (transactionResponseResult.TransactionResponse.Messages != null)
                    {
                        await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                            "Event Payment Processed");
                    }
                    else
                    {

                        if (transactionResponseResult.TransactionResponse.Errors != null)
                        {

                            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                                "Event Payment Failed " + transactionResponseResult.TransactionResponse.Errors[0].ErrorCode);

                            if (!await _waRepo.DeleteEventRegistration(eventRegistrationId))
                            {
                                throw new ExecutionError(
                                    "Transaction Failed and event Registration Cleanup Failed. Please contact our Help Desk to complete your registration");
                            }

                            throw new ExecutionError(transactionResponseResult.TransactionResponse.Errors[0].ErrorText);
                        }


                    }
                }
                else
                {
                    await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                        "Event Payment Failed " + transactionResponseResult.Messages.Message[0].Code);

                    if (!await _waRepo.DeleteEventRegistration(eventRegistrationId))
                    {
                        throw new ExecutionError(
                            "Transaction Failed and event Registration Cleanup Failed. Please contact our Help Desk to complete your registration");
                    }

                    if (transactionResponseResult.TransactionResponse != null && transactionResponseResult.TransactionResponse.Errors != null)
                    {
                        await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                            "Event Payment Failed " + transactionResponseResult.TransactionResponse.Errors[0].ErrorCode);
                        throw new ExecutionError(transactionResponseResult.TransactionResponse.Errors[0].ErrorText);
                    }


                    throw new ExecutionError(transactionResponseResult.Messages.Message[0].Text);
                }
            }

            //Create payment for invoice
            var paymentId = await _waRepo.MarkInvoiceAsPaid(registrationTypeDetails, invoiceId, contact.Id);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit,
                $"Invoice Marked as Paid - {paymentId}");

            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Registration Complete - {eventRegistrationId}");

            //Return Event Registration Id and QR Code Bitmap
            return new Sponsor()
            {
                EventRegistrationId = eventRegistrationId,
            };
        }
    }
}
