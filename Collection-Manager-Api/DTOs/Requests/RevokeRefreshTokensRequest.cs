namespace Collection_Manager_Api.DTOs.Requests;

public class RevokeRefreshTokensRequest
{
    public required string UserId { get; init; }
}