using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Responses;
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
    public async Task<BaseResponse<AuthenticationResponse>> Login(LoginRequest request, HttpRequestMeta httpRequestMeta, CancellationToken token)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new BaseResponse<AuthenticationResponse>() { Succeeded =  false , Data =  null , Message = "Invalid email or password" };
        }

        var accessToken = await _tokenProviderService.CreateAccessTokenAsync(user);

        var refreshToken = new RefreshToken()
        {
            Id = Guid.CreateVersion7().ToString(),
            UserId = user.Id,
            Token = _tokenProviderService.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
            UserAgent = httpRequestMeta.UserAgent,
            RemoteIpV4Address = httpRequestMeta.IpV4Address,
            RemoteIpV6Address = httpRequestMeta.IpV6Address,
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
        
        return new BaseResponse<AuthenticationResponse>() { Succeeded =  true , Data = authenticationResponse, Message = $"Login successful" };
    }

    public async Task<BaseResponse<TokenResponse>> LoginWithRefreshToken(LoginRefreshTokenRequest request, CancellationToken token)
    {
        RefreshToken? refreshToken = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken, token);

        if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
        { 
            return new BaseResponse<TokenResponse>() { Succeeded =  false , Data =  null , Message = "Invalid refresh token", Errors = "Invalid refresh token" };
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
        
        return new BaseResponse<TokenResponse>() { Succeeded = true, Data = tokenResponse, Message = "Login successful" };
    }

    public async Task<BaseResponse<List<RefreshTokenResponse>>> GetRefreshTokens(Guid id, CancellationToken token)
    {
        var list = await _context.RefreshTokens.Where(r => r.UserId == id.ToString()).ToListAsync(token);
        var listOfRefreshTokens = new List<RefreshTokenResponse>();
        list.ForEach(r =>
        {
            listOfRefreshTokens.Add(r.MapToRefreshTokenResponse());
        });
        return new BaseResponse<List<RefreshTokenResponse>>
        {
            Succeeded = true,
            Data = listOfRefreshTokens
        };
    }

    public async Task<bool> RevokeRefreshTokens(Guid id, CancellationToken token)
    {
       await _context.RefreshTokens.Where(r => r.UserId == id.ToString()).ExecuteDeleteAsync(token);
       return true;
    }

    public async Task<bool> RevokeRefreshToken(Guid id, CancellationToken token)
    {
        await _context.RefreshTokens.Where(r => r.Id == id.ToString()).ExecuteDeleteAsync(token);
        return true;
    }

    public async Task<bool> Logout(LogoutRefreshTokenRequest request,CancellationToken token)
    {
        await _context.RefreshTokens.Where(r => r.Token == request.RefreshToken).ExecuteDeleteAsync(token);
        return true;
    }

}