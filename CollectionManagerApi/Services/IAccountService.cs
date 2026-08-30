using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public interface IAccountService
{
    Task<BaseResponse<User>> Register(RegisterUser request, CancellationToken token);
    Task<BaseResponse<User>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<User>>> GetAll(CancellationToken token);
}