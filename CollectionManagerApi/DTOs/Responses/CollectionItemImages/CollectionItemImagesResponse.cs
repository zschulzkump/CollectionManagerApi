namespace CollectionManagerApi.DTOs.Responses.CollectionItemImages;

public class CollectionItemImagesResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CollectionItemId { get; set; }  = string.Empty;
    public string S3ImageId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsThumbnail { get; set; } = false;
    
    public string? PreSignedUrl { get; set; }
    public string DateCreated { get; set; } = string.Empty;
    public string DateModified { get; set; } = string.Empty;
}