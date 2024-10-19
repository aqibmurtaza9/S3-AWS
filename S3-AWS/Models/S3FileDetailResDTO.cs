namespace S3_AWS.Models
{
    public record S3FileDetailResDTO
    (
        int Sno,
        string Name,
        string Type,
        DateTime LastModified,
        string Size
    );
}
