using System.ComponentModel.DataAnnotations;

namespace CollectionManagerApi.Models;

public class CollectionItem
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; }
    [Required]
    public string CollectionGroupId { get; set; }
    [Required]
    public string StorageId { get; set; }
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    
    public string? Comments { get; set; } = string.Empty;
    [Required]
    public bool HasDocuments { get; set; } = false;
    [Required]
    public bool IsDamaged { get; set; } = false;
    [Required]
    public bool IsOnDisplay { get; set; } = false;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
    
    public User User { get; set; }
    public CollectionGroup CollectionGroup { get; set; }

    public Storage Storage { get; set; }
    
    public ICollection<CollectionItemCustomAttribute> CollectionItemsCustomAttributes { get; set; }
    
    public ICollection<CollectionItemTag> CollectionItemTags { get; set; }
    public ICollection<Tag> Tags { get; set; }
    
    public ICollection<ItemDocument> Documents { get; set; }
    public ICollection<ItemImage> Images { get; set; }
    
}