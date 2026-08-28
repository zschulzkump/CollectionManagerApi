using System.ComponentModel.DataAnnotations;

namespace Collection_Manager_Api.Models;

public class ItemImage
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    public string S3ImageId { get; set; } = string.Empty;
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public bool IsThumbnail { get; set; } = false;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
}