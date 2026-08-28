using System.ComponentModel.DataAnnotations;

namespace Collection_Manager_Api.Models;

public class CollectionItemCustomAttribute
{
    [Key]
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string DataType { get; set; } = string.Empty;
    [Required]
    public string Value { get; set; } = string.Empty;
    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Required]
    public DateTime DateModified { get; set; } = DateTime.Now;
}