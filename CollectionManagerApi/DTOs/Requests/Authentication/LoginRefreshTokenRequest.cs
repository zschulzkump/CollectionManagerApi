namespace CollectionManagerApi.DTOs.Requests.Authentication;

public class LoginRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}