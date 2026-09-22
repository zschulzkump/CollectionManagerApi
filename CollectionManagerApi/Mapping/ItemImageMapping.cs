using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using CollectionManagerApi.DTOs.Responses.CollectionItemImages;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class ItemImageMapping
{
    public static CollectionItemImagesResponse MapToCollectionItemImagesResponse(this ItemImage request, string? preSignedUrl = null)
    {
        return new CollectionItemImagesResponse
        {
            Id = request.Id,
            UserId = request.UserId,
            CollectionItemId =  request.CollectionItemId,
            S3ImageId =  request.S3ImageId,
            Name = request.Name,
            IsThumbnail =  request.IsThumbnail,
            PreSignedUrl = preSignedUrl,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static ItemImage MapToItemImage(this CreateCollectionItemImagesRequest request, string id, string userId, string collectionItemId, string s3ImageId)
    {
        return new ItemImage()
        {
            Id = id,
            UserId = userId,
            CollectionItemId = collectionItemId,
            S3ImageId = s3ImageId,
            Name = request.Name,
            IsThumbnail = request.IsThumbnail,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static ItemImage UpdateItemImage(this ItemImage current, UpdateCollectionItemImagesRequest newRequest)
    {
        current.Name = newRequest.Name;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}