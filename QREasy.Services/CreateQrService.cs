using Microsoft.Exchange.WebServices.Data;
using QRCoder;
using QREasy.Services.Interface;
using System.Drawing;
using System.Reflection;


namespace QREasy.Services
{
    public class CreateQrService : ICreateQrService
    {
        public CreateQrService()
        {
        }
        public byte[] CreateQrCode(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentException("Link cannot be empty.");
            }

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                            return ms.ToArray();
                        }
                    }
                }
            }
        }
    }
}
