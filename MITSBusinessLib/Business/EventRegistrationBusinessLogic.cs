using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MITSBusinessLib.Repositories.Interfaces;
using MITSBusinessLib.ResponseModels.Authorize;
using MITSBusinessLib.Utilities;
using MITSDataLib.Models;

namespace MITSBusinessLib.Business
{
    public class EventRegistrationBusinessLogic : IEventRegistrationBusinessLogic
    {

        private readonly IWaRepository _waRepo;
        private readonly IEventsRepository _eventsRepository;
        private readonly string _name;
        private readonly string _transactionKey;

        public EventRegistrationBusinessLogic(IWaRepository waRepo, IEventsRepository eventsRepository, IConfiguration config)
        {
            _waRepo = waRepo;
            _eventsRepository = eventsRepository;
            _name = config.GetSection("Secrets:Name").Value;
            _transactionKey = config.GetSection("Secrets:TransactionKey").Value;
        }

        public async Task<Registration> RegisterAttendee(Registration newRegistration)
        {
            var registrationTypeDetails = await _eventsRepository.GetEventTypeById(newRegistration.RegistrationTypeId);

            //var newLineItem = new LineItem
            //{
            //    ItemId = "0555",
            //    Name = $"Invoice #{0555}",
            //    Description = $"Registration for {registrationTypeDetails.Name}",
            //    Quantity = "1",
            //    UnitPrice = registrationTypeDetails.BasePrice.ToString(CultureInfo.InvariantCulture)
            //};

            //var lineItemList = new List<LineItem>
            //{
            //    newLineItem
            //};

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
                            InvoiceNumber = "00055",
                            Description = $"Registration for {registrationTypeDetails.Name}"

                        },
                        LineItems = new LineItems
                        {
                            LineItem = new LineItem
                            {
                                ItemId = "0555",
                                Name = $"Invoice #{0555}",
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
                Newtonsoft.Json.JsonConvert.DeserializeObject<TransactionResponse>(transactionResponseContent);

            return new Registration()
            {
                EventRegistrationId = 324234,
                QrCode = "324j2o3kj423ijd23n23ij923jd923jd2938jd2398du2398du2398dj2398"
            };
        }

    }
}
