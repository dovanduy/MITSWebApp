using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using QRCoder;

namespace MITSBusinessLib.Utilities
{
    public static class QrOps
    {
        public static string GenerateQrCode(int eventRegistrationId)
        {
            var imgType = Base64QRCode.ImageType.Jpeg;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(eventRegistrationId.ToString(), QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20, Color.Black, Color.White, true, imgType);

            return qrCodeImageAsBase64;
        }
    }
}
