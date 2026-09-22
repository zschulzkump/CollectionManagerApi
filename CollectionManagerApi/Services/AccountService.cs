using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Account;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Account;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    private readonly IConfiguration _configuration;

    private readonly UserManager<User> _userManager;

    public AccountService(
        ApplicationDbContext context,
        IConfiguration configuration,
        UserManager<User> userManager
    )
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;

    }
    public async Task<BaseResponse<UserResponse>> Register(RegisterUser request, CancellationToken token = default)
    {
        try
        {
            var userCount = await _context.Users.CountAsync(token);
            var createAdmin = (userCount == 0) ? true : false;
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken: token);
            var newUser = new User
            {   
                Id = Guid.CreateVersion7().ToString(),
                UserName =  request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var identityResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!identityResult.Succeeded)
            {
                return new BaseResponse<UserResponse>() {Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, createAdmin ? Roles.Admin: Roles.User);
            if (!addToRoleResult.Succeeded)
            {
                return new BaseResponse<UserResponse>() {Data = null, Succeeded =  false, StatusCode = InternalStatusCodes.DbStatus1501InsertError};
            }
            await transaction.CommitAsync(token);
            return new  BaseResponse<UserResponse>() {Data = newUser.MapToUserResponse(), Succeeded = true, Message = "User created", StatusCode = InternalStatusCodes.DbStatus1201Created};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<UserResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<UserResponse>> GetById(Guid id, CancellationToken token)
    {
        try
        {
            var identityResult = await _userManager.FindByIdAsync(id.ToString());
            if (identityResult == null)
            {
                return new BaseResponse<UserResponse>() {Data = null, Succeeded = false, Message = "User not found", StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            }
            return new BaseResponse<UserResponse>() {Data = identityResult.MapToUserResponse(), Succeeded = true, StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<UserResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
    
    public async Task<BaseResponse<List<User>>> GetAll(CancellationToken token)
    {
        try
        {
            var identityResult = await _userManager.Users.ToListAsync(token);
            if (identityResult.Count == 0)
            {
                return new BaseResponse<List<User>>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.DbStatus1404NotFound};
            }
            return new BaseResponse<List<User>>() {Data = identityResult, Succeeded = true,StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<List<User>>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

}