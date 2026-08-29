using Collection_Manager_Api.DTOs.Requests;
using Collection_Manager_Api.DTOs.Responses;
using Collection_Manager_Api.Models;

namespace Collection_Manager_Api.Services;

public interface IAccountService
{
    Task<BaseResponse<User>> Register(RegisterUser request, CancellationToken token);
    Task<BaseResponse<User>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<User>>> GetAll(CancellationToken token);
}