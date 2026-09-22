namespace CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;

public class CollectionItemAttributeFullResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CustomAttributeId { get; set; } = string.Empty;
    public string CollectionItemId { get; set; } = string.Empty;
    public string CustomAttributeName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}