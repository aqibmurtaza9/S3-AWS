using S3_AWS.Services.Interface;

namespace S3_AWS.Services
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfiguration configuration) 
        {
            BucketName = configuration["AWS:BucketName"];
            Region = configuration["AWS:Region"];
            AccessKeyID = configuration["AWS:AccessKeyID"];
            SecretAccessKey = configuration["AWS:SecretKey"];
            SessionToken = configuration["AWS:SessionToken"];
        }

        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AccessKeyID { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }
    }
}
