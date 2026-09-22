using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CustomAttribute;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CustomAttributeService : ICustomAttributeService
{
    readonly ApplicationDbContext _context;
    
    public CustomAttributeService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse<CustomAttributeResponse>> Create(Guid userId, CreateCustomAttributeRequest request, CancellationToken token)
    {
        try
        {
            var customAttribute = request.MapToCustomAttribute(id: Guid.CreateVersion7().ToString(), userId: userId.ToString());
            _context.CustomAttributes.Add(customAttribute);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CustomAttributeResponse> { Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            return new BaseResponse<CustomAttributeResponse>() {Data = customAttribute.MapToCustomAttributeResponse(), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CustomAttributeResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var result = await _context.CustomAttributes.FindAsync(id.ToString(), token);
            if (result == null) return new BaseResponse<CustomAttributeResponse>(){Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<CustomAttributeResponse>() {Data = result.MapToCustomAttributeResponse(), StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CustomAttributeResponse>> GetByName(string name, CancellationToken token)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<CustomAttributeResponse>>> GetAll(Guid userId, CancellationToken token)
    {
        try
        {
            var results = await _context.CustomAttributes
                .Where(x => x.UserId == userId.ToString())
                .Select(x => x.MapToCustomAttributeResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CustomAttributeResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CustomAttributeResponse>>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CustomAttributeResponse>> Update(Guid userId, Guid id, UpdateCustomAttributeRequest request, CancellationToken token)
    {
        try
        {
            var customAttribute = await _context.CustomAttributes.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (customAttribute == null) return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newCustomAttribute = customAttribute.UpdateCustomAttribute(request);
            _context.CustomAttributes.Update(newCustomAttribute);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CustomAttributeResponse>() {Data = newCustomAttribute.MapToCustomAttributeResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CustomAttributeResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.CustomAttributes.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.CustomAttributes.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CustomAttributeResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}