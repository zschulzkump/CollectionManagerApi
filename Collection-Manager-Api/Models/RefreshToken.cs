namespace Collection_Manager_Api.Models;

public class RefreshToken
{
    public string Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public string? UserAgent { get; set; }
    public string? RemoteIpV4Address { get; set; }
    public string? RemoteIpV6Address { get; set; }
    
    public User User { get; set; }
}