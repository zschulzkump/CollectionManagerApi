using System.ComponentModel.DataAnnotations;

namespace CollectionManagerApi.Models;

public class Tag
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
    
    public ICollection<CollectionItemTag> CollectionItemTags { get; set; }
    public ICollection<CollectionItem> CollectionItems { get; set; }
    public User User { get; set; }
}