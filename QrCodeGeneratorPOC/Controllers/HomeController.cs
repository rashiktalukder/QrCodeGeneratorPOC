using Microsoft.AspNetCore.Mvc;
using QrCodeGeneratorPOC.Models;
using QRCoder;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using static QRCoder.PayloadGenerator;

namespace QrCodeGeneratorPOC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new QrCodeModel());
        }

        [HttpPost]
        public ActionResult Index(QrCodeModel qrCodeModel)
        {
            string modelInfo = $"Product Name: {qrCodeModel.ProductName}" +
                $"\n\nProduct Type: {qrCodeModel.ProductType}" +
                $"\n\nProduct Description: {qrCodeModel.ProductDescription}";

            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(modelInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeAsBitmap = qrCode.GetGraphic(20);

            qrCodeModel.QrImageUrl = $"data:image/png;base64,{Convert.ToBase64String(BitmapToByteArray(qrCodeAsBitmap))}";

            return View(qrCodeModel);
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
