using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Collections;

namespace CollectionManagerApi.Services;

public interface ICollectionGroupService
{
    Task<BaseResponse<CollectionGroupResponse>> Create(Guid userId, CreateCollectionGroupRequest request, CancellationToken token);
    Task<BaseResponse<CollectionGroupResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<CollectionGroupResponse>> GetByName(string name, CancellationToken token);
    Task<BaseResponse<List<CollectionGroupResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<CollectionGroupResponse>> Update(Guid userId, Guid id, UpdateCollectionGroupRequest request, CancellationToken token);
    Task<BaseResponse<CollectionGroupResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}