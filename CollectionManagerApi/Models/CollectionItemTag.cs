namespace CollectionManagerApi.Models;

public class CollectionItemTag
{
    public string CollectionItemId { get; set; } = string.Empty;
    public string TagId { get; set; } = string.Empty;
    
    public CollectionItem CollectionItem { get; set; } = new CollectionItem();
    public Tag Tag { get; set; } = new Tag();
}