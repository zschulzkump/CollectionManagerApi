namespace Collection_Manager_Api.DTOs.Requests;

public class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}