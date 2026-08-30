namespace CollectionManagerApi.DTOs.Requests;

public class LoginRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}