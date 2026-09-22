using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Account;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Account;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public interface IAccountService
{
    Task<BaseResponse<UserResponse>> Register(RegisterUser request, CancellationToken token);
    Task<BaseResponse<UserResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<User>>> GetAll(CancellationToken token);
}