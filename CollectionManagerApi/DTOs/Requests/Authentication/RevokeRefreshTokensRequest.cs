namespace CollectionManagerApi.DTOs.Requests.Authentication;

public class RevokeRefreshTokensRequest
{
    public required string UserId { get; init; }
}