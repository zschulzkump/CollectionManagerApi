using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Responses;
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
    public async Task<BaseResponse<User>> Register(RegisterUser request, CancellationToken token = default)
    {
        var userCount = await _context.Users.CountAsync(token);
        var createAdmin = (userCount == 0) ? true : false;
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken: token);
        var newUser = new User
        {   
            Id = Guid.CreateVersion7().ToString(),
            UserName =  request.Email,
            Email = request.Email
        };
        var identityResult = await _userManager.CreateAsync(newUser, request.Password);
        if (!identityResult.Succeeded)
        {
            return new BaseResponse<User>() {Data = null, Succeeded =  false, Message = identityResult.Errors.FirstOrDefault()?.Description!, Errors = identityResult.Errors.FirstOrDefault()?.Description};
        }
        var addToRoleResult = await _userManager.AddToRoleAsync(newUser, createAdmin ? Roles.Admin: Roles.User);
        if (!addToRoleResult.Succeeded)
        {
            return new BaseResponse<User>() {Data = null, Succeeded =  false, Message = addToRoleResult.Errors.FirstOrDefault()?.Description!, Errors = addToRoleResult.Errors.FirstOrDefault()?.Description};
        }
        await transaction.CommitAsync(token);
        return new  BaseResponse<User>() {Data = null, Succeeded = true, Message = "User created"};
    }

    public async Task<BaseResponse<User>> GetById(Guid id, CancellationToken token)
    {
        var identityResult = await _userManager.FindByIdAsync(id.ToString());
        if (identityResult == null)
        {
            return new BaseResponse<User>() {Data = null, Succeeded = false, Message = "User not found"};
        }
        return new BaseResponse<User>() {Data = identityResult, Succeeded = true};
    }
    
    public async Task<BaseResponse<List<User>>> GetAll(CancellationToken token)
    {
        var identityResult = await _userManager.Users.ToListAsync(token);
        if (identityResult.Count == 0)
        {
            return new BaseResponse<List<User>>() {Data = null, Succeeded = false, Message = "No users found"};
        }
        return new BaseResponse<List<User>>() {Data = identityResult, Succeeded = true};
    }

}