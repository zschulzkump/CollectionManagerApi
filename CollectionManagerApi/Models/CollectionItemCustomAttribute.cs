using System.ComponentModel.DataAnnotations;

namespace CollectionManagerApi.Models;

public class CollectionItemCustomAttribute
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string UserId { get; set; } = string.Empty;
    public string CustomAttributeId { get; set; } = string.Empty;
    public string CollectionItemId { get; set; } = string.Empty;
    [Required]
    public string DataType { get; set; } = string.Empty;
    [Required]
    public string Value { get; set; } = string.Empty;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
    public User User { get; set; }
    public CustomAttribute CustomAttribute { get; set; }
    public CollectionItem CollectionItem { get; set; }
    
}