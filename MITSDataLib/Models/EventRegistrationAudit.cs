using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class EventRegistrationAudit
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public string RegistrationTypeId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Status will be used to track if there are any errors. Will track the last successful step
        //Starting - Received Process Registration Request
        //Payment Processed - Authorize.net payment was successfully processed
        //Contact Created - Contact was created in Wild Apricot
        //Event Registration Created - Attendee was successfully registered for Event
        //Event Invoice Created - Invoice was successfully created for event registration
        //Event Payment Created - Payment was successfully created for Invoice
        //Process Complete - QR Code was created and everything worked as it should

        public string Status { get; set; }
        public DateTime Modified { get; set; }
    }
}
