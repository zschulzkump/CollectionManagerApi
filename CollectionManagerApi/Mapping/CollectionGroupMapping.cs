using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using CollectionManagerApi.DTOs.Responses.Collections;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class CollectionGroupMapping
{
    public static CollectionGroupResponse MapToCollectionGroupResponse(this CollectionGroup request)
    {
        return new CollectionGroupResponse()
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Name,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    public static CollectionGroup MapToCollectionGroup(this CreateCollectionGroupRequest request, string id, string userId)
    {
        return new CollectionGroup()
        {
            Id = id,
            UserId = userId,
            Name = request.Name,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static CollectionGroup UpdateCollectionGroup(this CollectionGroup current, UpdateCollectionGroupRequest newRequest)
    {
        current.Name = newRequest.Name;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}