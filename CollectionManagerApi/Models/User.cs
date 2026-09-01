using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CollectionManagerApi.Models;

public class User: IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<CollectionGroup> CollectionGroups { get; set; } = new List<CollectionGroup>();
    public ICollection<CollectionItem> CollectionItems { get; set; } = new List<CollectionItem>();
    public ICollection<CustomAttribute> CustomAttributes { get; set; } = new List<CustomAttribute>();
    public ICollection<CollectionItemCustomAttribute> CollectionItemCustomAttributes { get; set; } = new List<CollectionItemCustomAttribute>();
    public ICollection<ItemDocument> ItemDocuments { get; set; } = new List<ItemDocument>();
    public ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();
    public ICollection<Storage> Storages { get; set; } = new List<Storage>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
}