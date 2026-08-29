namespace Collection_Manager_Api.DTOs.Requests;

public class LoginTokenRequest
{
    public required string RefreshToken { get; init; }
}