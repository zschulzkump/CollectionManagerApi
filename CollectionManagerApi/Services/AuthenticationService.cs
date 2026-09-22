using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Authentication;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerApi.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ITokenProviderService _tokenProviderService;
    
    public AuthenticationService(UserManager<User> userManager, ITokenProviderService tokenProviderService, ApplicationDbContext context)
    {
        _userManager = userManager;
        _tokenProviderService = tokenProviderService;
        _context = context;
    }
    public async Task<BaseResponse<AuthenticationResponse>> Login(LoginRequest request, CancellationToken token)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
        
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new BaseResponse<AuthenticationResponse>() { Succeeded =  false , Data =  null , Message = "Invalid email or password", StatusCode = InternalStatusCodes.DbStatus1404NotFound };
            }

            var accessToken = await _tokenProviderService.CreateAccessTokenAsync(user);
            var keepSignedInDays = int.TryParse(Environment.GetEnvironmentVariable("KEEP_SIGNED_IN_DAYS"),  out var result) ? result : 1 ;
            var refreshToken = new RefreshToken()
            {
                Id = Guid.CreateVersion7().ToString(),
                UserId = user.Id,
                Token = _tokenProviderService.GenerateRefreshToken(),
                ExpiresOnUtc = DateTime.UtcNow.AddDays(keepSignedInDays),
            };
        

            var authenticationResponse = new AuthenticationResponse()
            {
                UserId = user.Id,
                AuthenticationTokens = new TokenResponse()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token,
                }
            };
        
            _context.RefreshTokens.Add(refreshToken);
        
            await _context.SaveChangesAsync(token);
        
            return new BaseResponse<AuthenticationResponse>() { Succeeded =  true , Data = authenticationResponse, Message = $"Login successful", StatusCode = InternalStatusCodes.DbStatus1200Ok };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<AuthenticationResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<TokenResponse>> LoginWithRefreshToken(LoginRefreshTokenRequest request, CancellationToken token)
    {
        try
        {
            RefreshToken? refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == request.RefreshToken, token);

            if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            { 
                return new BaseResponse<TokenResponse>() { Succeeded =  false , Data =  null , Message = "Invalid refresh token", Errors = "Invalid refresh token", StatusCode = InternalStatusCodes.AuthStatus3501TokenExpired };
            }
        
            string accessToken = await _tokenProviderService.CreateAccessTokenAsync(refreshToken.User);
            refreshToken.Token = _tokenProviderService.GenerateRefreshToken();
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

            var tokenResponse = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };
        
            await _context.SaveChangesAsync(token);
        
            return new BaseResponse<TokenResponse>() { Succeeded = true, Data = tokenResponse, Message = "Login successful", StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<TokenResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
    
    public async Task<BaseResponse<RefreshTokenResponse>> RevokeRefreshTokens(Guid id, CancellationToken token)
    {
        try
        {
            await _context.RefreshTokens.Where(r => r.UserId == id.ToString()).ExecuteDeleteAsync(token);
            return new BaseResponse<RefreshTokenResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<RefreshTokenResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }

    public async Task<BaseResponse<RefreshTokenResponse>> Logout(LogoutRefreshTokenRequest request,CancellationToken token)
    {
        try
        {
            await _context.RefreshTokens.Where(r => r.Token == request.RefreshToken).ExecuteDeleteAsync(token);
            return new BaseResponse<RefreshTokenResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.DbStatus1200Ok};
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<RefreshTokenResponse>() {Data = null, Succeeded = false, Message = "No users found", StatusCode = InternalStatusCodes.Status5500UnknownError};
        }
    }
}