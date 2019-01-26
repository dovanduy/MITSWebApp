using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using MimeKit.Utils;
using MITSDataLib.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MITSBusinessLib.Utilities
{

    public interface IMailOps
    {
        void Send(int registrationId, string registrantGuid, Registration registration,
            WildApricotRegistrationType waRegistrationType, string registrationGuid);
    }

    public class MailOps : IMailOps
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _pasword;

        public MailOps(IConfiguration config )
        {
           _server = config.GetSection("EmailConfiguration:Server").Value;
           _port = int.Parse(config.GetSection("EmailConfiguration:Port").Value);
           _userName = config.GetSection("EmailConfiguration:Username").Value;
           _pasword = config.GetSection("EmailConfiguration:Password").Value;
        }

        public void Send(int registrationId, string registrantGuid, Registration registration, WildApricotRegistrationType waRegistrationType, string registrationGuid)
        {

            var ticketRef = $"https://test.com/tickets/{registrantGuid}/ticket.html";

            var message = new MimeMessage();
            message.To.Add(new MailboxAddress("John Smith", registration.Email));
            message.From.Add(new MailboxAddress("MITS 2019", "AFCEAMITS2019@gmail.com"));
            message.Subject = $"Your {waRegistrationType.Name} Ticket";
            


            var builder = new BodyBuilder();
            const string ticketFileName = "email.html";
            var baseTicketDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "baseticket");
            var content = File.ReadAllText(baseTicketDirectory + "\\" + ticketFileName);

            content = content.Replace("{registration_id}", registrationId.ToString());
            content = content.Replace("{event_name}", waRegistrationType.Name);
            content = content.Replace("{event_cost}", waRegistrationType.BasePrice.ToString());
            content = content.Replace("{ticket_href}", ticketRef);
           

            builder.HtmlBody = content;

 


            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_server, _port);
                client.Authenticate(_userName, _pasword);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Send(message);
                client.Disconnect(true);
            }

        }
        
    }
}
