using Microsoft.AspNetCore.Mvc;

namespace AnimalShelter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");

            // Check if directory exists
            if (!Directory.Exists(folderPath))
            {
                return NotFound(new
                {
                    error = "Not Found",
                    message = "Directory not found."
                }
                );
            }

            // Get all files in the directory
            var fileNames = Directory.GetFiles(folderPath)
                                      .Select(Path.GetFileName)
                                      .ToList();

            return Ok(new { files = fileNames });
        }

        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var filePath = Path.Combine("wwwroot/files", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new
                {
                    error = "Not Found",
                    message = "File not found."
                });
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "File is empty or not provided."
                });
            }

            if (file.ContentType != "application/pdf")
            {
                return BadRequest(new
                {
                    error = "Bad Request",
                    message = "Only PDF files are allowed."
                });
            }

            var uploadsPath = Path.Combine("wwwroot/files");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var filePath = Path.Combine(uploadsPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                status = "Ok",
                message = "File uploaded successfully.",
                fileName = file.FileName
            });
        }
    }
}