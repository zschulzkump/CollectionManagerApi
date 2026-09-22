namespace CollectionManagerApi.DTOs.Requests.CollectionItemImages;

public class CreateCollectionItemImagesRequest
{
    public string Name { get; set; }
    public bool IsThumbnail { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}