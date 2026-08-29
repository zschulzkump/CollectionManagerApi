using Collection_Manager_Api.Models;

namespace Collection_Manager_Api.Services;

public interface ITokenProviderService
{
    Task<string> CreateAccessTokenAsync(User user);
    string GenerateRefreshToken();
}