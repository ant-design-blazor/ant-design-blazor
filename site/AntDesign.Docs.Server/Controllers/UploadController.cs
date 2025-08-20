using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AntDesign.Docs.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            var fileName = file.FileName;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                success = true,
                message = "File uploaded successfully",
                fileName = fileName,
                filePath = $"/uploads/{uniqueFileName}"
            });
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were uploaded.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var results = new List<object>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var fileName = file.FileName;
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                results.Add(new
                {
                    success = true,
                    message = "File uploaded successfully",
                    fileName = fileName,
                    filePath = $"/uploads/{uniqueFileName}"
                });
            }

            return Ok(new
            {
                success = true,
                message = "All files uploaded successfully",
                files = results
            });
        }
    }
} 