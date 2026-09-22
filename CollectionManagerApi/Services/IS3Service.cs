using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public interface IS3Service
{
    Task<BaseResponse<S3ServiceResult>> UploadItemImage(Guid id, CreateCollectionItemImagesRequest request, CancellationToken token);
    Task<BaseResponse<S3ServiceResult>> GetItemImageByKey(string key, CancellationToken token);
    Task<BaseResponse<S3ServiceResult>> DeleteItemImageByKey(string key, CancellationToken token);
    Task<BaseResponse<S3ServiceResult>> UploadItemDocument(Guid id, CreateCollectionItemDocumentsRequest request, CancellationToken token);
    Task<BaseResponse<S3ServiceResult>> GetItemDocumentByKey(string key, CancellationToken token);
    Task<BaseResponse<S3ServiceResult>> DeleteItemDocumentByKey(string key, CancellationToken token);
}