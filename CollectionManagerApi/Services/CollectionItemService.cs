using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItem;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItems;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CollectionItemService : ICollectionItemService
{
    private readonly ApplicationDbContext _context;
    
    public CollectionItemService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse<CollectionItemResponse>> Create(Guid userId, Guid collectionGroupId, CreateCollectionItemRequest request, CancellationToken token)
    {
        try
        {
            var collectionItem = request.MapToCollectionItem(id: Guid.CreateVersion7().ToString(), userId: userId.ToString(), collectionGroupId: collectionGroupId.ToString());
            _context.CollectionItems.Add(collectionItem);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)
                return new BaseResponse<CollectionItemResponse>()
                    { Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1501InsertError };
            return new BaseResponse<CollectionItemResponse>() { Data = collectionItem.MapToCollectionItemResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1201Created };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }

    }

    public async Task<BaseResponse<CollectionItemResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var result = await _context.CollectionItems.FindAsync(id.ToString(), token); ;
            if (result == null) return new BaseResponse<CollectionItemResponse>() { Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound };
            return new BaseResponse<CollectionItemResponse>() {Data = result.MapToCollectionItemResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }

    }

    public async Task<BaseResponse<CollectionItemResponse>> GetByName(string name, CancellationToken token)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
    
    public async Task<BaseResponse<List<CollectionItemResponse>>> GetAllByCollectionId(Guid userId, Guid collectionGroupId, CancellationToken token)
    {
        try
        {
            var results = await _context.CollectionItems
                .Where(x => x.UserId == userId.ToString() && x.CollectionGroupId == collectionGroupId.ToString())
                .Select(x => x.MapToCollectionItemResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CollectionItemResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionItemResponse>>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<CollectionItemResponse>>> GetAll(Guid userId, CancellationToken token)
    {
        try
        {
            var results = await _context.CollectionItems
                .Where(x => x.UserId == userId.ToString())
                .Select(x => x.MapToCollectionItemResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CollectionItemResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionItemResponse>>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemResponse>> Update(Guid userId, Guid id, UpdateCollectionItemRequest request, CancellationToken token)
    {
        try
        {
            var currentCollectionItem = await _context.CollectionItems.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (currentCollectionItem == null) return new BaseResponse<CollectionItemResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newCollectionItem = currentCollectionItem.UpdateCollectionItem(newRequest: request);
            _context.CollectionItems.Update(newCollectionItem);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CollectionItemResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CollectionItemResponse>() {Data = newCollectionItem.MapToCollectionItemResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.CollectionItems.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CollectionItemResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.CollectionItems.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<CollectionItemResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        };
    }
}