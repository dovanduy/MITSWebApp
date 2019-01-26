using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using MITSDataLib.Models;
using QRCoder;

namespace MITSBusinessLib.Utilities
{
    public static class TicketOps
    {
        public static string GenerateBase64QrCode(int eventRegistrationId)
        {
            var imgType = Base64QRCode.ImageType.Jpeg;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(eventRegistrationId.ToString(), QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20, Color.Black, Color.White, true, imgType);

            return qrCodeImageAsBase64;
        }

        public static MemoryStream GenerateBitmapQrCodeSteam(int eventRegistrationId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(eventRegistrationId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);

            var stream = new MemoryStream();
            qrCodeImage.Save(stream, ImageFormat.Png);

            stream.Position = 0;

            return stream;

        }


        public static string GenerateTicket(int eventRegistrationId, Registration registration, WildApricotRegistrationType waRegistrationType)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(eventRegistrationId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);

            var registrantGuid = Guid.NewGuid().ToString("N");

            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tickets", registrantGuid);
            Directory.CreateDirectory(directory);

            qrCodeImage.Save(directory + "\\code.png");

            const string ticketFileName = "ticket.html";
            var baseTicketDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "baseticket");
            var content = File.ReadAllText(baseTicketDirectory + "\\" +  ticketFileName);
            content = content.Replace("{first_name}", registration.FirstName);
            content = content.Replace("{last_name}", registration.LastName);
            content = content.Replace("{registration_type}", waRegistrationType.Name);
            content = content.Replace("{registration_id}", eventRegistrationId.ToString());
            content = content.Replace("{event_date}", waRegistrationType.WaEvent.StartDate.ToShortDateString());

            var ticketdirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tickets", registrantGuid);

            File.WriteAllText(ticketdirectory + "\\ticket.html", content);

            

            return registrantGuid;

        }

    }
}
