using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;
using CollectionManagerApi.DTOs.Responses.CustomAttribute;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CollectionItemAttributeService : ICollectionItemAttributeService
{
    private readonly ApplicationDbContext _context;
    public CollectionItemAttributeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<CollectionItemAttributeResponse>> Create(Guid collectionItemId, Guid userId, CreateCollectionItemAttributeRequest request, CancellationToken token)
    {
        try
        {
            var collectionItemAttribute = request.MapToCollectionItemAttribute(id: Guid.CreateVersion7().ToString(), userId: userId.ToString(), collectionItemId: collectionItemId.ToString());
            _context.CollectionItemCustomAttributes.Add(collectionItemAttribute);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemAttributeResponse> { Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = collectionItemAttribute.MapToCollectionItemAttributeResponse(), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemAttributeResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var result = await _context.CollectionItemCustomAttributes.FindAsync(id.ToString(), token);
            if (result == null) return new BaseResponse<CollectionItemAttributeResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = result.MapToCollectionItemAttributeResponse(), StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<CollectionItemAttributeFullResponse>>> GetAll(Guid userId, CancellationToken token)
    {
        try
        {
            var results = await _context.CollectionItemCustomAttributes
                .Where(x => x.UserId == userId.ToString())
                .LeftJoin(
                    _context.CustomAttributes,
                    collectionItemCustomAttributes => collectionItemCustomAttributes.CustomAttributeId,
                    customAttributes => customAttributes.Id, (collectionItemCustomAttributes, customAttributes) => new CollectionItemAttributeFullResponse()
                        {
                            Id = collectionItemCustomAttributes.Id,
                            UserId = collectionItemCustomAttributes.UserId,
                            CollectionItemId = collectionItemCustomAttributes.CollectionItemId,
                            CustomAttributeId = collectionItemCustomAttributes.CustomAttributeId,
                            CustomAttributeName = (string?)customAttributes!.Name ?? "not available",
                            DataType = collectionItemCustomAttributes.DataType,
                            Value = collectionItemCustomAttributes.Value,
                            DateCreated =  collectionItemCustomAttributes.DateCreated,
                            DateModified = collectionItemCustomAttributes.DateModified,
                        })
                .ToListAsync(token);
            return new BaseResponse<List<CollectionItemAttributeFullResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionItemAttributeFullResponse>>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemAttributeResponse>> Update(Guid userId, Guid id, UpdateCollectionItemAttributeRequest request, CancellationToken token)
    {
        try
        {
            var collectionItemCustomAttribute = await _context.CollectionItemCustomAttributes.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (collectionItemCustomAttribute == null) return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newcollectionItemCustomAttribute = collectionItemCustomAttribute.UpdateCollectionItemAttribute(request);
            _context.CollectionItemCustomAttributes.Update(newcollectionItemCustomAttribute);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = newcollectionItemCustomAttribute.MapToCollectionItemAttributeResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemAttributeResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.CollectionItemCustomAttributes.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.CollectionItemCustomAttributes.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}