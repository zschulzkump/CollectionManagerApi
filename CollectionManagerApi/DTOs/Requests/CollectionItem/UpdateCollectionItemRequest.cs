namespace CollectionManagerApi.DTOs.Requests.CollectionItem;

public class UpdateCollectionItemRequest
{
    public required string CollectionGroupId { get; set; } = string.Empty;
    public required string StorageId { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public string? Comments { get; set; } = string.Empty;
    public required bool HasDocuments { get; set; } = false;
    public required bool IsDamaged { get; set; } = false;
    public required bool IsOnDisplay { get; set; } = false;
}