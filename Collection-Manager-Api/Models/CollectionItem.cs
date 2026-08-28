using System.ComponentModel.DataAnnotations;

namespace Collection_Manager_Api.Models;

public class CollectionItem
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
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
}