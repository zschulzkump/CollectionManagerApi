using Collection_Manager_Api.DTOs.Responses;
using Collection_Manager_Api.Models;

namespace Collection_Manager_Api.Mapping;

public static class Mappings
{
    public static UserResponse MapToUserResponse(this User request)
    {
        return new UserResponse()
        {
            UserId = request.Id,
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
        };
    }
    
    public static RefreshTokenResponse MapToRefreshTokenResponse(this RefreshToken request)
    {
        return new RefreshTokenResponse()
        {
            Id = request.Id,
            Token = request.Token,
        };
    }
}