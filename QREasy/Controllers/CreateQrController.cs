using Microsoft.AspNetCore.Mvc;
using QREasy.Services.Interface;

namespace QREasy.Controllers
{
    public class CreateQrController : Controller
    {
        private readonly ICreateQrService _createQrService;

        public CreateQrController(ICreateQrService createQrService)
        {
            _createQrService = createQrService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQrImage(string link)
        {
            try
            {
                // Zaman damgası ile benzersiz bir dosya adı oluştur
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"QRCode_{timestamp}.png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // QR kodunu oluştur
                var qrCode = _createQrService.CreateQrCode(link);

                // QR kodunu dosyaya kaydet
                System.IO.File.WriteAllBytes(filePath, qrCode);

                // Dosya yolunu döndür
                var relativeFilePath = $"/images/{fileName}";
                return Json(new { qrCodeUrl = relativeFilePath, fileName = fileName });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        public IActionResult GetQrImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Dosyayı göster
            return File(fileBytes, "image/png", fileName);
        }

        [HttpPost]
        public IActionResult DeleteQrImage(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(); // Silme başarılı
                }
                else
                {
                    return NotFound("QR code image not found.");
                }
            }
            catch (Exception ex)
            {
                // Daha ayrıntılı hata mesajı
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
