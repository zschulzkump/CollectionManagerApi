using CollectionManagerApi.DTOs.Requests.Storages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Storages;

namespace CollectionManagerApi.Services;

public interface IStorageService
{
    Task<BaseResponse<StorageResponse>> Create(Guid userId, StorageRequest request, CancellationToken token);
    Task<BaseResponse<StorageResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<StorageResponse>> GetByName(string name, CancellationToken token);
    Task<BaseResponse<List<StorageResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<StorageResponse>> Update(Guid userId, Guid id, StorageRequest request, CancellationToken token);
    Task<BaseResponse<StorageResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}