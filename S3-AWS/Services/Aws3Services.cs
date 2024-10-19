using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http.HttpResults;
using S3_AWS.Models;
using S3_AWS.Services.Interface;
using System.Security.Cryptography.X509Certificates;

namespace S3_AWS.Services
{
    public class Aws3Services : IAws3Services
    {
        public IAppConfiguration _configuration { get; set; }


        public Aws3Services(IAppConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public Task<bool> DeleteFileAsync(string fileName, string versionId = "")
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadFileAsync(string file)
        {
            throw new NotImplementedException();
        }

        public async Task<(int StatusCode, string Message)> UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return (StatusCodes.Status400BadRequest, "Invalid file. File cannot be null or empty.");
                }

                var allowedExtensions = new[] { ".jpg", ".png", ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return (StatusCodes.Status403Forbidden, "File type not allowed. Please upload a valid file.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = stream,
                        Key = file.FileName, 
                        BucketName = _configuration.BucketName,
                        ContentType = file.ContentType
                    };

                    var transferUtility = new TransferUtility(S3ClientVerification());
                    await transferUtility.UploadAsync(uploadRequest);
                }
                return (StatusCodes.Status201Created, "File uploaded successfully.");
            }
            catch (AmazonS3Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, $"Error encountered on server. Message: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private AmazonS3Client S3ClientVerification()
        {
            var credentials = new BasicAWSCredentials(_configuration.AccessKeyID, _configuration.SecretAccessKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };

            var client = new AmazonS3Client(credentials, config);
            return client;
        }

        public async Task<List<S3Object>> GetAllS3FilesAsync()
        {
            AmazonS3Client Client = S3ClientVerification();
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = _configuration.BucketName
            };

            ListObjectsV2Response response = await Client.ListObjectsV2Async(request);
            return response.S3Objects;
        }

        public async Task<MemoryStream> DownloadFileFromS3Async(string fileName)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _configuration.BucketName,
                Key = fileName
            };

            var client = S3ClientVerification();

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (Stream responseStream = response.ResponseStream)
                {
                    var memoryStream = new MemoryStream();
                    await responseStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset the position for returning
                    return memoryStream;
                }
            }
        }

    }
}
