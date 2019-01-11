using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using QRCoder;

namespace MITSBusinessLib.Utilities
{
    public static class QrOps
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

        public static Bitmap GenerateBitmapQrCode(int eventRegistrationId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(eventRegistrationId.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, true);

            return qrCodeImage;

        }

    }
}
