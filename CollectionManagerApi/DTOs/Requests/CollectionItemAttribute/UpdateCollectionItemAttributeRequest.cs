namespace CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;

public class UpdateCollectionItemAttributeRequest
{
    public string CustomAttributeId { get; set; } = string.Empty;
    public string CollectionItemId { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}