using CollectionManagerApi.DTOs.Requests.CollectionItem;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItems;


namespace CollectionManagerApi.Services;

public interface ICollectionItemService
{
    Task<BaseResponse<CollectionItemResponse>> Create(Guid userId, Guid collectionGroupId, CreateCollectionItemRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<CollectionItemResponse>> GetByName(string name, CancellationToken token);
    Task<BaseResponse<List<CollectionItemResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<CollectionItemResponse>> Update(Guid userId, Guid id, UpdateCollectionItemRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}