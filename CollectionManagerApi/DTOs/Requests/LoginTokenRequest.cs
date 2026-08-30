namespace CollectionManagerApi.DTOs.Requests;

public class LoginTokenRequest
{
    public required string RefreshToken { get; init; }
}