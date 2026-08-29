using System.ComponentModel.DataAnnotations;

namespace Collection_Manager_Api.Models;

public class Category
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
    
    public ICollection<CollectionItem> CollectionItems { get; set; } = new List<CollectionItem>();
}