namespace CollectionManagerApi.DTOs.Requests.Tags;

public class CreateTagRequest
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
}