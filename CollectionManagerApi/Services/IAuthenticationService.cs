using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Authentication;

namespace CollectionManagerApi.Services;

public interface IAuthenticationService
{
    Task<BaseResponse<AuthenticationResponse>> Login(LoginRequest request, CancellationToken token);
    Task<BaseResponse<TokenResponse>> LoginWithRefreshToken(LoginRefreshTokenRequest request, CancellationToken token);
    Task<BaseResponse<RefreshTokenResponse>> RevokeRefreshTokens(Guid id, CancellationToken token);
    Task<BaseResponse<RefreshTokenResponse>> Logout(LogoutRefreshTokenRequest request,CancellationToken token);
}