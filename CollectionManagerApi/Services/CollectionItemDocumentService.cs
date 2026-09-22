using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemDocuments;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class CollectionItemDocumentService : ICollectionItemDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    public CollectionItemDocumentService(ApplicationDbContext context, IS3Service s3Service)
    {
        _context = context;
        _s3Service = s3Service;
    }

    public async Task<BaseResponse<CollectionItemDocumentsResponse>> Create(Guid userId, Guid collectionItemId, CreateCollectionItemDocumentsRequest request, CancellationToken token)
    {
        try
        {
            var itemDocumentId = Guid.CreateVersion7();
            var s3Result = await _s3Service.UploadItemDocument(itemDocumentId, request, token);
            if (s3Result.Data == null) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
            var itemDocument = request.MapToItemDocument(id: itemDocumentId.ToString(), userId: userId.ToString(), collectionItemId: collectionItemId.ToString(), s3Result.Data.Key);
            _context.ItemDocuments.Add(itemDocument);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemDocumentsResponse> { Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = itemDocument.MapToCollectionItemDocumentsResponse(preSignedUrl: s3Result.Data.PreSignedUrl), Succeeded = true, StatusCode =  InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemDocumentsResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var itemDocument = await _context.ItemDocuments.FindAsync(id.ToString(), token);
            if (itemDocument == null) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            var s3Results = await _s3Service.GetItemDocumentByKey(itemDocument.S3ImageId.ToString(), token);
            if (s3Results.Data == null) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.S3Status2500ServerError};
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = itemDocument.MapToCollectionItemDocumentsResponse(preSignedUrl: s3Results.Data.PreSignedUrl), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<List<CollectionItemDocumentsResponse>>> GetAll(Guid userId, Guid collectionItemId, CancellationToken token)
    {
        try
        {
            var results = await _context.ItemDocuments
                .Where(x => x.UserId == userId.ToString())
                .Where(x =>x.CollectionItemId == collectionItemId.ToString())
                .Select(x => x.MapToCollectionItemDocumentsResponse())
                .ToListAsync(token);
            return new BaseResponse<List<CollectionItemDocumentsResponse>>() { Data = results, Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<CollectionItemDocumentsResponse>>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemDocumentsResponse>> Update(Guid userId, Guid id, UpdateCollectionItemDocumentsRequest request, CancellationToken token)
    {
        try
        {
            var currentCollectionItemDocument = await _context.ItemDocuments.Where(x => x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (currentCollectionItemDocument == null) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode =  InternalStatusCodes.DbStatus1404NotFound};
            var newCollectionItemDocument = currentCollectionItemDocument.UpdateItemDocument(newRequest: request);
            _context.ItemDocuments.Update(newCollectionItemDocument);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0)return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1502UpdateError};
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = newCollectionItemDocument.MapToCollectionItemDocumentsResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<CollectionItemDocumentsResponse>> Delete(Guid userId, Guid id, CancellationToken token)
    {
        try
        {
            var entity = await _context.ItemDocuments.Where(x => x.UserId == userId.ToString() && x.Id == id.ToString()).FirstOrDefaultAsync(token);
            if (entity == null) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            _context.ItemDocuments.Remove(entity);
            var rowsAffected = await _context.SaveChangesAsync(token);
            if (rowsAffected == 0) return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.DbStatus1503DeleteError};
            await _s3Service.DeleteItemDocumentByKey(entity.S3ImageId, token);
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = entity.MapToCollectionItemDocumentsResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<CollectionItemDocumentsResponse>() {Data = null, Succeeded = false, StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}