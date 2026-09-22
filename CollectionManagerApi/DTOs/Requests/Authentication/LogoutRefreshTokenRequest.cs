namespace CollectionManagerApi.DTOs.Requests.Authentication;

public class LogoutRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}