using System.ComponentModel.DataAnnotations;

namespace Collection_Manager_Api.Models;

public class CustomAttribute
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
    
    public ICollection<CollectionItemCustomAttribute> CollectionItemsCustomAttributes { get; set; } = new List<CollectionItemCustomAttribute>();
}