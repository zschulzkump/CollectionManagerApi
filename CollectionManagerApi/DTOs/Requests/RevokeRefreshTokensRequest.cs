namespace CollectionManagerApi.DTOs.Requests;

public class RevokeRefreshTokensRequest
{
    public required string UserId { get; init; }
}