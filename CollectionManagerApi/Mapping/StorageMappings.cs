using CollectionManagerApi.DTOs.Requests.Storages;
using CollectionManagerApi.DTOs.Responses.Storages;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Mapping;

public static class StorageMappings
{
    public static StorageResponse MapToStorageResponse(this Storage request)
    {
        return new StorageResponse()
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Name,
            DateCreated = request.DateCreated.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
            DateModified = request.DateModified.ConvertDateTimeFromUtcToLocal().ToString("MM/dd/yyyy hh:mm:ss tt"),
        };
    }
    
    public static Storage MapToStorage(this StorageRequest request, string id, string userId)
    {
        return new Storage()
        {
            Id = id,
            UserId = userId,
            Name = request.Name,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow
        };
    }
    
    public static Storage UpdateStorage(this Storage current, StorageRequest newRequest)
    {
        current.Name = newRequest.Name;
        current.DateModified = DateTime.UtcNow;
        return current;
    }
}