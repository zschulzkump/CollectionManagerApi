using CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;
using CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class CollectionItemAttributeMapping
{
    public static CollectionItemAttributeResponse MapToCollectionItemAttributeResponse(this CollectionItemCustomAttribute request)
    {
        return new CollectionItemAttributeResponse
        {
            Id = request.Id,
            CustomAttributeId =  request.CustomAttributeId,
            CollectionItemId =  request.CollectionItemId,
            DataType = request.DataType,
            Value = request.Value,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static CollectionItemCustomAttribute MapToCollectionItemAttribute(this CreateCollectionItemAttributeRequest request, string id, string userId, string collectionItemId)
    {
        return new CollectionItemCustomAttribute()
        {
            Id = id,
            UserId = userId,
            CustomAttributeId = request.CustomAttributeId,
            CollectionItemId = collectionItemId,
            DataType = request.DataType,
            Value = request.Value,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static CollectionItemCustomAttribute UpdateCollectionItemAttribute(this CollectionItemCustomAttribute current, UpdateCollectionItemAttributeRequest newRequest)
    {
        current.CustomAttributeId = newRequest.CustomAttributeId;
        current.CollectionItemId = newRequest.CollectionItemId;
        current.DataType = newRequest.DataType;
        current.Value = newRequest.Value;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}