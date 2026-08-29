namespace Collection_Manager_Api.DTOs.Requests;

public class LogoutRefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}