using CollectionManagerApi.DTOs.Requests.CollectionItem;
using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using CollectionManagerApi.DTOs.Responses.CustomAttribute;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class CustomAttributeMapping
{
     public static CustomAttributeResponse MapToCustomAttributeResponse(this CustomAttribute request)
    {
        return new CustomAttributeResponse
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Name,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static CustomAttribute MapToCustomAttribute(this CreateCustomAttributeRequest request, string id, string userId)
    {
        return new CustomAttribute()
        {
            Id = id,
            UserId = userId,
            Name = request.Name,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static CustomAttribute UpdateCustomAttribute(this CustomAttribute current, UpdateCustomAttributeRequest newRequest)
    {
        current.Name = newRequest.Name;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}