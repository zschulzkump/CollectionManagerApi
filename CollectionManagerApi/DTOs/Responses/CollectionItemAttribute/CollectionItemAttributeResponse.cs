using CollectionManagerApi.DTOs.Responses.CustomAttribute;

namespace CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;

public class CollectionItemAttributeResponse
{
    public string Id { get; set; } = string.Empty;
    public string CustomAttributeId { get; set; } = string.Empty;
    public string CollectionItemId { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string DateCreated { get; set; } = string.Empty;
    public string DateModified { get; set; } = string.Empty;
}