namespace Collection_Manager_Api.DTOs.Responses;

public class AuthenticationResponse
{
    public required string UserId { get; init; }
    public required TokenResponse AuthenticationTokens { get; init; }
}