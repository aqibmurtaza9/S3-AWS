namespace S3_AWS.Services.Interface
{
    public interface IAppConfiguration
    {
        string AccessKeyID { get; set; }
        string SecretAccessKey { get; set; }
        string SessionToken { get; set; }
        string BucketName { get; set; }
        string Region { get; set; }
    }
}
