using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses.CollectionItemDocuments;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class CollectionItemDocumentsMappings
{
    public static CollectionItemDocumentsResponse MapToCollectionItemDocumentsResponse(this ItemDocument request, string? preSignedUrl = null)
    {
        return new CollectionItemDocumentsResponse
        {
            Id = request.Id,
            UserId = request.UserId,
            CollectionItemId = request.CollectionItemId,
            S3ImageId = request.S3ImageId,
            Name = request.Name,
            PreSignedUrl = preSignedUrl,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt")
        };
    }
    
    public static ItemDocument MapToItemDocument(this CreateCollectionItemDocumentsRequest request, string id, string userId, string collectionItemId, string s3ImageId)
    {
        return new ItemDocument()
        {
            Id = id,
            UserId = userId,
            CollectionItemId = collectionItemId,
            S3ImageId = s3ImageId,
            Name = request.Name,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static ItemDocument UpdateItemDocument(this ItemDocument current, UpdateCollectionItemDocumentsRequest newRequest)
    {
        current.Name = newRequest.Name;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}