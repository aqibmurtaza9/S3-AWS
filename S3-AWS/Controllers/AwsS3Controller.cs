using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using S3_AWS.Services;
using S3_AWS.Services.Interface;
using System.Net;

namespace S3_AWS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AwsS3Controller : ControllerBase
    {
        public IAws3Services _aws3Services { get; set; }
        public AwsS3Controller(IAws3Services aws3Services)
        {
            _aws3Services = aws3Services;
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocumentToS3(IFormFile file)
        {
            try
            {
                var (status, message) = await _aws3Services.UploadFileAsync(file);
                return StatusCode(status, message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<List<Amazon.S3.Model.S3Object>> GetAllS3Files()
        {
            var list = await _aws3Services.GetAllS3FilesAsync();
            return list;
        }

        [HttpGet("getFile")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var fileStream = await _aws3Services.DownloadFileFromS3Async(fileName);
            return File(fileStream, "application/octet-stream", fileName);
        }
    }
}
