namespace CollectionManagerApi.DTOs.Responses.Authentication;

public class TokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}