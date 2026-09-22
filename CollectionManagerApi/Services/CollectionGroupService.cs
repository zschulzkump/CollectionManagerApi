using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Collections;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CollectionGroupService : ICollectionGroupService
{
    private readonly ApplicationDbContext _context;
    
    public CollectionGroupService(
        ApplicationDbContext context
        )
    {
        _context = context;
    }
    public async Task<BaseResponse<CollectionGroupResponse>> Create(Guid userId, CreateCollectionGroupRequest request, CancellationToken token)
    {
        try
        {
            var collectionGroup = request.MapToCollectionGroup(id: Guid.CreateVersion7().ToString(), userId: userId.ToString());
            _context.CollectionGroups.Add(collectionGroup);
            var rowsAffected  = await _context.SaveChangesAsync(token);
            return rowsAffected == 0 ?
                new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1501InsertError} :
                new BaseResponse<CollectionGroupResponse>() {Data = collectionGroup.MapToCollectionGroupResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1201Created};
        }
        catch (DbUpdateException  ex)
        {
            Console.WriteLine($"Database save failed: {ex.Message}");
            return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionGroupResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var result = await _context.CollectionGroups.FindAsync(id.ToString(), token);
            if (result == null) return new BaseResponse<CollectionGroupResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<CollectionGroupResponse>() {Data = result.MapToCollectionGroupResponse(), StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionGroupResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionGroupResponse>> GetByName(string name, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<List<CollectionGroupResponse>>> GetAll(Guid userId,CancellationToken token)
    {
        try
        {
            var results = await _context.CollectionGroups
                .Where(x => x.UserId == userId.ToString())
                .Select(x => x.MapToCollectionGroupResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CollectionGroupResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionGroupResponse>>() { Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError };
        }
    }

    public async Task<BaseResponse<CollectionGroupResponse>> Update(Guid userId, Guid id, UpdateCollectionGroupRequest request, CancellationToken token)
    {
        try
        {
            var collectionGroup = await _context.CollectionGroups.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (collectionGroup == null) return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newCollectionGroup = collectionGroup.UpdateCollectionGroup(request);
            _context.CollectionGroups.Update(newCollectionGroup);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CollectionGroupResponse>() {Data = newCollectionGroup.MapToCollectionGroupResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionGroupResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.CollectionGroups.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.CollectionGroups.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionGroupResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}