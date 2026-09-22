using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Account;
using CollectionManagerApi.DTOs.Responses.Authentication;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class Mappings
{
    public static RefreshTokenResponse MapToRefreshTokenResponse(this RefreshToken request)
    {
        return new RefreshTokenResponse()
        {
            Id = request.Id,
            Token = request.Token,
        };
    }
}