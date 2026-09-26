using CollectionManagerApi.DTOs.Requests.Storages;
using CollectionManagerApi.DTOs.Requests.Tags;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Tags;

namespace CollectionManagerApi.Services;

public interface ITagsService
{
    Task<BaseResponse<TagsResponse>> Create(Guid userId, Guid collectionItemId, CreateTagRequest request, CancellationToken token);
    Task<BaseResponse<TagsResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<TagsResponse>>> GetAllByCollectionId(Guid userId, Guid collectionItemId, CancellationToken token);
    Task<BaseResponse<List<TagsResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<TagsResponse>> Update(Guid userId, Guid id, UpdateTagRequest request, CancellationToken token);
    Task<BaseResponse<TagsResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}