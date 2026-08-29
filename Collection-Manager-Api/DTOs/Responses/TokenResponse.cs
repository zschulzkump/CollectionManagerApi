namespace Collection_Manager_Api.DTOs.Responses;

public class TokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}