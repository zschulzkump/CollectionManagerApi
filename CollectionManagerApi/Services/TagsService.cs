using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.Tags;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Tags;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class TagsService : ITagsService
{
    private readonly ApplicationDbContext _context;
    
    public TagsService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse<TagsResponse>> Create(Guid userId, Guid collectionItemId, CreateTagRequest request, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            
            var tag = request.MapToTag(userId.ToString());
            var collectionItemTag = tag.CreateCollectionItemTag(collectionItemId);
            if (request.Id == null) _context.Tags.Add(tag);
            _context.CollectionItemTags.Add(collectionItemTag);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)
            {
                return new BaseResponse<TagsResponse>()
                    { Data = null, StatusCode = InternalStatusCodes.DbStatus1501InsertError };
            }
            await transaction.CommitAsync(token);
            return new BaseResponse<TagsResponse>()
                { Data = tag.MapToTagsResponse(), StatusCode = InternalStatusCodes.DbStatus1201Created };
        }
        catch (Exception e)
        {
            if (e is DbUpdateException or DbUpdateConcurrencyException or OperationCanceledException)
            { 
                await transaction.RollbackAsync(token);
            }
            Console.WriteLine(e);
            return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<TagsResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var tag = await _context.Tags.Where(t => t.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (tag == null)  return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<TagsResponse>() {Data = tag.MapToTagsResponse(), StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<TagsResponse>>> GetAllByCollectionId(Guid userId, Guid collectionItemId, CancellationToken token)
    {
        try
        {
            var tags = await _context.Tags
                .Where(t => t.UserId == userId.ToString() && t.CollectionItemTags.Any(e => e.CollectionItemId == collectionItemId.ToString()))
                .Select(tag => tag.MapToTagsResponse())
                .ToListAsync(token);
            if (tags.Count == 0) return new BaseResponse<List<TagsResponse>>() {Data =  null, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<List<TagsResponse>>() {Data = tags,  StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<TagsResponse>>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<TagsResponse>>> GetAll(Guid userId, CancellationToken token)
    {
        try
        {
            var tags = await _context.Tags
                .Where(t => t.UserId == userId.ToString())
                .Select(tag => tag.MapToTagsResponse())
                .ToListAsync(token);
            if (tags.Count == 0) return new BaseResponse<List<TagsResponse>>() {Data =  null, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            return new BaseResponse<List<TagsResponse>>() {Data = tags,  StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<TagsResponse>>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<TagsResponse>> Update(Guid userId, Guid id, UpdateTagRequest request, CancellationToken token)
    {
        try
        {
            var currentTag = await _context.Tags.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (currentTag == null) return new BaseResponse<TagsResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newTag = currentTag.UpdateTag(newRequest: request);
            _context.Tags.Update(newTag);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<TagsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<TagsResponse>() {Data = newTag.MapToTagsResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<TagsResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.Tags.Where(x=>x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<TagsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            var collectionItemTag = await _context.CollectionItemTags.Where(e => e.TagId == id.ToString()).FirstOrDefaultAsync(token);
            if (collectionItemTag == null) return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.CollectionItemTags.Remove(collectionItemTag);
            _context.Tags.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<TagsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            return new BaseResponse<TagsResponse>() {Data = null, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<TagsResponse>() {Data = null, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}