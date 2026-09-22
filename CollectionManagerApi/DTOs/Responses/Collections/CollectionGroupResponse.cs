namespace CollectionManagerApi.DTOs.Responses.Collections;

public class CollectionGroupResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DateCreated { get; set; }
    public string DateModified { get; set; }
}