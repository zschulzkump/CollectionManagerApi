namespace CollectionManagerApi.DTOs.Requests;

public class LogoutRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}