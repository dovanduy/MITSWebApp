using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using MITSBusinessLib.ResponseModels.Authorize;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MITSBusinessLib.Utilities
{
    public static class AuthorizeOps
    {

        private static readonly string AuthorizeUrl = "https://apitest.authorize.net/xml/v1/request.api";

        public static async Task<HttpResponseMessage> CreateTransaction(ProcessTransaction processTransaction)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var transactionRequestString = JsonConvert.SerializeObject(processTransaction, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });

            var content = new StringContent(transactionRequestString, Encoding.UTF8, "application/json");

            return await client.PostAsync(AuthorizeUrl, content);

        }

        //{
        //    "createTransactionRequest": {
        //        "merchantAuthentication": {
        //            "name": "84EchY4vL",
        //            "transactionKey": "2vBW32bv953Jd947"
        //        },
        //        "transactionRequest": {
        //            "transactionType": "authCaptureTransaction",
        //            "amount": "5",
        //            "payment": {
        //                "opaqueData": {
        //                    "dataDescriptor": "COMMON.ACCEPT.INAPP.PAYMENT",
        //                    "dataValue": "eyJjb2RlIjoiNTBfMl8wNjAwMDUzRDY2MjE1QzFBQUVCOUY2QjU5N0NFMDlFMEZGMjU1RUUxMUUzQzUwQzZEMUIzNkYzOUJEMEUzMjdBMTQ4MTI0ODUzRjNBODBCMUU5OURGMEVDRDkzNjMzMTM4M0U1NDk1IiwidG9rZW4iOiI5NTQ2Mjg4OTYxNDEzODM4MzA0NjAzIiwidiI6IjEuMSJ9"
        //                }
        //            },
        //            "order" : {
        //                "invoiceNumber": "0555",
        //                "description" : "Registration for an Event"
        //            },
        //            "lineItems": {
        //                "lineItem": {
        //                    "itemId": "0555",
        //                    "name": "Invoice #0555",
        //                    "description": "Registration for AFCEA January Luncheon ",
        //                    "quantity": "1",
        //                    "unitPrice": "450.00"
        //                }
        //            },
        //            "billTo": {
        //                "firstName": "Ellen",
        //                "lastName": "Johnson",
        //                "company": "Souveniropolis",
        //                "address": "14 Main Street",
        //                "city": "Pecan Springs",
        //                "state": "TX",
        //                "zip": "44628",
        //                "country": "USA"
        //            }
        //        }
        //    }
        //}
    }
}
