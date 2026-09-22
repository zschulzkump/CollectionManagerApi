using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Account;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class UserMapping
{
    public static UserResponse MapToUserResponse(this User request)
    {
        return new UserResponse()
        {
            UserId = request.Id,
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
    }
}