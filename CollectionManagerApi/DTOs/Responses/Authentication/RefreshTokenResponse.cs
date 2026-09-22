namespace CollectionManagerApi.DTOs.Responses.Authentication;

public class RefreshTokenResponse
{
    public required string Id { get; set; }
    public required string Token { get; set; }
}