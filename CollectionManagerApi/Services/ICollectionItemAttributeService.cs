using CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;

namespace CollectionManagerApi.Services;

public interface ICollectionItemAttributeService
{
    Task<BaseResponse<CollectionItemAttributeResponse>> Create(Guid collectionItemId, Guid userId, CreateCollectionItemAttributeRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemAttributeResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<CollectionItemAttributeFullResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<CollectionItemAttributeResponse>> Update(Guid userId, Guid id, UpdateCollectionItemAttributeRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemAttributeResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}