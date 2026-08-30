using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public interface ITokenProviderService
{
    Task<string> CreateAccessTokenAsync(User user);
    string GenerateRefreshToken();
}