namespace Collection_Manager_Api.DTOs.Requests;

public class LoginRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}