using Amazon.S3.Model;

namespace S3_AWS.Services.Interface
{
    public interface IAws3Services
    {
        Task<byte[]> DownloadFileAsync(string file);
        Task<(int StatusCode, string Message)> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileName, string versionId = "");
        Task<List<S3Object>> GetAllS3FilesAsync();
        Task<MemoryStream> DownloadFileFromS3Async(string fileName);
    }
}
