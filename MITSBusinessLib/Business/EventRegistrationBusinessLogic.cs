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

            //Retrieve Contact from WildApricot
            //Create Contact if needed
            var contact = await _waRepo.GetContact(newRegistration.Email) ?? await _waRepo.CreateContact(newRegistration);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Contact Created - {contact.Id}");
            

            //Create Event Registration

            var eventRegistrationId = await _waRepo.AddEventRegistration(newRegistration, contact.Id);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Event Registration Created - {eventRegistrationId}");

            //Create Invoice

            var invoiceId = await _waRepo.GenerateEventRegistrationInvoice(eventRegistrationId);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Event Invoice Created - {invoiceId}");

            //Process Payment... What happens if the payment fails? Will the user be able to register again? What happens to their other event registration?

            var registrationTypeDetails = await _eventsRepository.GetEventTypeById(newRegistration.RegistrationTypeId);

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
                    }
                }
            };

            var transactionResponse = await AuthorizeOps.CreateTransaction(processTransaction);

            var transactionResponseContent = await transactionResponse.Content.ReadAsStringAsync();

            var transactionResponseResult =
                Newtonsoft.Json.JsonConvert.DeserializeObject<CreateTransactionResponse>(transactionResponseContent);

            //handle all the errors in the transactionResponseresult

            if (transactionResponseResult != null)
            {
                if (transactionResponseResult.TransactionResponse.Messages != null)
                {
                    await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, "Event Payment Processed");
                    Console.WriteLine("The transaction was successful");
                }
            }
            else
            {
                //throw error
            }

            //Create payment for invoice
            var paymentId = await _waRepo.MarkInvoiceAsPaid(registrationTypeDetails, invoiceId, contact.Id);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Invoice Marked as Paid - {paymentId}");

            //Create QR Code

            var qrCode = QrOps.GenerateBase64QrCode(eventRegistrationId);
            await _registrationRepo.UpdateEventRegistrationAudit(eventRegistrationAudit, $"Registration Complete - {eventRegistrationId}");

            //Send email with Confirmation and QR code

            var guid = Guid.NewGuid().ToString("N");

            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tickets", guid);
            Directory.CreateDirectory(directory);
            var code = QrOps.GenerateBitmapQrCode(eventRegistrationId);
            code.Save(directory + "\\code.png");

            


            //_mailOps.Send(newRegistration.Email, eventRegistrationId, QrOps.GenerateBitmapQrCode(eventRegistrationId), qrCode);

            //Return Event Registration Id and QR Code Bitmap
            return new Registration()
            {
                EventRegistrationId = eventRegistrationId,
                QrCode = guid
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

    }
}
