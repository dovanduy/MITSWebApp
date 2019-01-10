using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using MimeKit.Utils;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MITSBusinessLib.Utilities
{

    public interface IMailOps
    {
       void Send(string toAddress, int registrationId, MemoryStream QRCode);
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

        public void Send(string toAddress, int registrationId, MemoryStream QRCode)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress("John Smith", toAddress));
            message.From.Add(new MailboxAddress("MITS 2019", "AFCEAMITS2019@gmail.com"));
            message.Subject = "This is the subject";
            

            var builder = new BodyBuilder();
            builder.Attachments.Add("Code.png", QRCode);
            var image = builder.LinkedResources.Add("Code.png", QRCode);
            image.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = string.Format(@"
            Hello,

You have successfully registered

 <img src=""cid:d0932d0823098d20d832"" >

                ", image.ContentId);

            
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
