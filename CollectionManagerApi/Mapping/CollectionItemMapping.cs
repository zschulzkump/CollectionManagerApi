using CollectionManagerApi.DTOs.Requests.CollectionItem;
using CollectionManagerApi.DTOs.Responses.CollectionItems;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class CollectionItemMapping
{
    public static CollectionItemResponse MapToCollectionItemResponse(this CollectionItem request)
    {
        return new CollectionItemResponse
        {
            Id = request.Id,
            UserId = request.UserId,
            CollectionGroupId = request.CollectionGroupId,
            StorageId = request.StorageId,
            Name = request.Name,
            Comments = request.Comments,
            HasDocuments = request.HasDocuments,
            IsDamaged = request.IsDamaged,
            IsOnDisplay = request.IsOnDisplay,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static CollectionItem MapToCollectionItem(this CreateCollectionItemRequest request, string id, string userId, string collectionGroupId)
    {
        return new CollectionItem()
        {
            Id = id,
            UserId = userId,
            CollectionGroupId = collectionGroupId,
            StorageId = request.StorageId,
            Name = request.Name,
            Comments = request.Comments,
            HasDocuments = request.HasDocuments,
            IsDamaged = request.IsDamaged,
            IsOnDisplay = request.IsOnDisplay,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static CollectionItem UpdateCollectionItem(this CollectionItem current, UpdateCollectionItemRequest newRequest)
    {
        current.CollectionGroupId = newRequest.CollectionGroupId;
        current.StorageId = newRequest.StorageId;
        current.Name = newRequest.Name;
        current.Comments = newRequest.Comments;
        current.HasDocuments = newRequest.HasDocuments;
        current.IsDamaged = newRequest.IsDamaged;
        current.IsOnDisplay = newRequest.IsOnDisplay;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}