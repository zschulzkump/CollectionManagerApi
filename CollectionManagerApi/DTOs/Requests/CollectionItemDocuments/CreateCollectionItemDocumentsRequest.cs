namespace CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;

public class CreateCollectionItemDocumentsRequest
{
    public string Name { get; set; } = string.Empty;
    public string FileName { get; set; }
    public string ContentType { get; set; }
}