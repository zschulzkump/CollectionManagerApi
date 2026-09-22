using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemImages;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CollectionItemImageService : ICollectionItemImageService
{
    private readonly ApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    public CollectionItemImageService(ApplicationDbContext context, IS3Service s3Service)
    {
        _context = context;
        _s3Service = s3Service;
    }
    public async Task<BaseResponse<CollectionItemImagesResponse>> Create(Guid userId, Guid collectionItemId, CreateCollectionItemImagesRequest request, CancellationToken token)
    {
        try
        {
            var itemImageId = Guid.CreateVersion7();
            var s3Result = await _s3Service.UploadItemImage(itemImageId, request, token);
            if (s3Result.Data == null) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
            var itemImage = request.MapToItemImage(id: itemImageId.ToString(), userId: userId.ToString(), collectionItemId: collectionItemId.ToString(), s3Result.Data.Key);
            _context.ItemImages.Add(itemImage);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemImagesResponse> { Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            return new BaseResponse<CollectionItemImagesResponse>() {Data = itemImage.MapToCollectionItemImagesResponse(preSignedUrl: s3Result.Data.PreSignedUrl), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemImagesResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var itemImage = await _context.ItemImages.FindAsync(id.ToString(), token);
            if (itemImage == null) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            var s3Results = await _s3Service.GetItemImageByKey(itemImage.S3ImageId.ToString(), token);
            if (s3Results.Data == null) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.S3Status2500ServerError};
            return new BaseResponse<CollectionItemImagesResponse>() {Data = itemImage.MapToCollectionItemImagesResponse(preSignedUrl: s3Results.Data.PreSignedUrl), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<CollectionItemImagesResponse>>> GetAll(Guid userId, Guid collectionItemId, CancellationToken token)
    {
        try
        {
            var results = await _context.ItemImages
                .Where(x => x.UserId == userId.ToString() && x.CollectionItemId == collectionItemId.ToString())
                .Select(x => x.MapToCollectionItemImagesResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CollectionItemImagesResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionItemImagesResponse>>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemImagesResponse>> Update(Guid userId, Guid id, UpdateCollectionItemImagesRequest request, CancellationToken token)
    {
        try
        {
            var currentCollectionItemImage = await _context.ItemImages.Where(x => x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (currentCollectionItemImage == null) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newCollectionItemImage = currentCollectionItemImage.UpdateItemImage(newRequest: request);
            _context.ItemImages.Update(newCollectionItemImage);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CollectionItemImagesResponse>() {Data = newCollectionItemImage.MapToCollectionItemImagesResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemImagesResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.ItemImages.Where(x => x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.ItemImages.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            await _s3Service.DeleteItemImageByKey(entity.S3ImageId, token);
            return new BaseResponse<CollectionItemImagesResponse>() {Data = entity.MapToCollectionItemImagesResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemImagesResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}