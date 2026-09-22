namespace CollectionManagerApi.DTOs.Requests.Authentication;

public class LoginTokenRequest
{
    public required string RefreshToken { get; init; }
}