using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Responses;

namespace CollectionManagerApi.Services;

public interface IAuthenticationService
{
    Task<BaseResponse<AuthenticationResponse>> Login(LoginRequest request, CancellationToken token);
    Task<BaseResponse<TokenResponse>> LoginWithRefreshToken(LoginRefreshTokenRequest request, CancellationToken token);
    Task<BaseResponse<List<RefreshTokenResponse>>> GetRefreshTokens(Guid id, CancellationToken token);
    Task<bool> RevokeRefreshTokens(Guid id, CancellationToken token);
    Task<bool> RevokeRefreshToken(Guid id, CancellationToken token);
    Task<bool> Logout(LogoutRefreshTokenRequest request,CancellationToken token);
}