using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemImages;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public interface ICollectionItemImageService
{
    Task<BaseResponse<CollectionItemImagesResponse>> Create(Guid userId, Guid collectionItemId, CreateCollectionItemImagesRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemImagesResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<CollectionItemImagesResponse>>> GetAll(Guid userId, Guid collectionItemId, CancellationToken token);
    Task<BaseResponse<CollectionItemImagesResponse>> Update(Guid userId, Guid id, UpdateCollectionItemImagesRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemImagesResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}