
using System.Globalization;
using CollectionManagerApi.DTOs.Requests.Tags;
using CollectionManagerApi.DTOs.Responses.Tags;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class TagMapping
{
    public static TagsResponse MapToTagsResponse(this Tag request)
    {
        return new TagsResponse()
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Name,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static Tag MapToTag(this CreateTagRequest request, string userId)
    {
        var id = request.Id ?? Guid.CreateVersion7();
        return new Tag()
        {
            Id = id.ToString(),
            UserId = userId,
            Name = request.Name.ToLowerInvariant(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static Tag UpdateTag(this Tag current, UpdateTagRequest newRequest)
    {
        current.Name = newRequest.Name.ToLowerInvariant();
        current.DateModified = DateTime.UtcNow;
        return current;
    }
    
    public static CollectionItemTag CreateCollectionItemTag(this Tag current, Guid collectionItemId)
    {
        return new CollectionItemTag()
        {
            CollectionItemId = collectionItemId.ToString(),
            TagId = current.Id,
        };
    }
}