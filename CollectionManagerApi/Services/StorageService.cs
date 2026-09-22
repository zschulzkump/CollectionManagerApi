using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.Storages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Storages;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class StorageService : IStorageService
{
    private readonly ApplicationDbContext _context;
    public StorageService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse<StorageResponse>> Create(Guid userId, StorageRequest request, CancellationToken token)
    {
        try
        {
            var storage = request.MapToStorage(id: Guid.CreateVersion7().ToString(), userId: userId.ToString());
            _context.Storages.Add(storage);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<StorageResponse> { Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            return new BaseResponse<StorageResponse>() {Data = storage.MapToStorageResponse(), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine($"Database save failed: {e.Message}");
            return new BaseResponse<StorageResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
        
    }

    public async Task<BaseResponse<StorageResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var result = await _context.Storages.FindAsync(id.ToString(), token);
            if (result == null) return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<StorageResponse>() {Data = result.MapToStorageResponse(), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<StorageResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<StorageResponse>> GetByName(string name, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<List<StorageResponse>>> GetAll(Guid userId, CancellationToken token)
    {
        try
        {
            var results = await _context.Storages
                .Where(x => x.UserId == userId.ToString())
                .Select(x => x.MapToStorageResponse())
                .ToListAsync(token);
            
            return new BaseResponse<List<StorageResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<StorageResponse>>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<StorageResponse>> Update(Guid userId, Guid id, StorageRequest request, CancellationToken token)
    {
        try
        {
            var storage = await _context.Storages.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
        
            if (storage == null) return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            var newStorage = storage.UpdateStorage(request);
            _context.Storages.Update(newStorage);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<StorageResponse>() {Data = newStorage.MapToStorageResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<StorageResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<StorageResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.Storages.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.Storages.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<StorageResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<StorageResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}