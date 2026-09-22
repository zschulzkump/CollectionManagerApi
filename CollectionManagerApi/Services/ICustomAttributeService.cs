using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CustomAttribute;

namespace CollectionManagerApi.Services;

public interface ICustomAttributeService
{
    Task<BaseResponse<CustomAttributeResponse>> Create(Guid userId,CreateCustomAttributeRequest request, CancellationToken token);
    Task<BaseResponse<CustomAttributeResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<CustomAttributeResponse>> GetByName(string name, CancellationToken token);
    Task<BaseResponse<List<CustomAttributeResponse>>> GetAll(Guid userId, CancellationToken token);
    Task<BaseResponse<CustomAttributeResponse>> Update(Guid userId, Guid id, UpdateCustomAttributeRequest request, CancellationToken token);
    Task<BaseResponse<CustomAttributeResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}