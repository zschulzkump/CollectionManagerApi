namespace CollectionManagerApi.DTOs.Responses.Authentication;

public class AuthenticationResponse
{
    public required string UserId { get; init; }
    public required TokenResponse AuthenticationTokens { get; init; }
}