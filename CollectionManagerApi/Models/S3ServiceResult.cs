namespace CollectionManagerApi.Models;

public class S3ServiceResult
{
    public required string Key { get; set; }
    public required string PreSignedUrl { get; set; }
}