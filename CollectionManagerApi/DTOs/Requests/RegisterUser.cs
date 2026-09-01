namespace CollectionManagerApi.DTOs.Requests;

public class RegisterUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}