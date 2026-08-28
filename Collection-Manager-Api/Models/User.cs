using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Collection_Manager_Api.Models;

public class User: IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}