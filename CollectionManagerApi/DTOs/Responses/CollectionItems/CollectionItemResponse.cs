namespace CollectionManagerApi.DTOs.Responses.CollectionItems;

public class CollectionItemResponse
{
    public required string Id { get; set; } = string.Empty;
    public required string UserId { get; set; } = string.Empty;
    public required string CollectionGroupId { get; set; } = string.Empty;
    public required string StorageId { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public string? Comments { get; set; } = string.Empty;
    public required bool HasDocuments { get; set; } = false;
    public required bool IsDamaged { get; set; } = false;
    public required bool IsOnDisplay { get; set; } = false;
    public required string DateCreated { get; set; }
    public required string DateModified { get; set; }
}