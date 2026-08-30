using System.ComponentModel.DataAnnotations;

namespace CollectionManagerApi.Models;

public class CollectionItem
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string CategoryId { get; set; } = string.Empty;
    [Required]
    public string StorageId { get; set; } = string.Empty;
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    
    public string Comments { get; set; } = string.Empty;
    [Required]
    public bool HasDocuments { get; set; } = false;
    [Required]
    public bool IsDamaged { get; set; } = false;
    [Required]
    public bool IsOnDisplay { get; set; } = false;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
    

    public Category Category { get; set; } = new Category();

    public Storage Storage { get; set; }  = new Storage();
    
    public ICollection<CollectionItemCustomAttribute> CollectionItemsCustomAttributes { get; set; } = new List<CollectionItemCustomAttribute>();
    
    public ICollection<CollectionItemTag> CollectionItemTags { get; set; } = new List<CollectionItemTag>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    public ICollection<ItemDocument> Documents { get; set; } = new List<ItemDocument>();
    public ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();
}