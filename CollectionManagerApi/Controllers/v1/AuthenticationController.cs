using System.Security.Claims;
using Asp.Versioning;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Authentication;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Authentication;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<LoginRequest> _loginInValidator;
    private readonly IValidator<LoginRefreshTokenRequest> _loginRefreshTokenValidator;
    private readonly IValidator<LogoutRefreshTokenRequest> _logoutRefreshTokenValidator;
    private readonly HttpContextAccessorHelper _httpContextAccessorHelper;
    public AuthenticationController(
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor,
        IValidator<LoginRequest> loginInValidator,
        IValidator<LoginRefreshTokenRequest> loginRefreshTokenValidator,
        IValidator<LogoutRefreshTokenRequest> logoutRefreshTokenValidator,
        HttpContextAccessorHelper httpContextAccessorHelper
        )
    {
        _authenticationService = authenticationService;
        _httpContextAccessor = httpContextAccessor;
        _loginInValidator = loginInValidator;
        _loginRefreshTokenValidator = loginRefreshTokenValidator;
        _logoutRefreshTokenValidator = logoutRefreshTokenValidator;
        _httpContextAccessorHelper = httpContextAccessorHelper;
    }
    
    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Authentication.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        try
        {
            var validationResult = await _loginInValidator.ValidateAsync(request, token);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await _authenticationService.Login(request, token);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => Ok(result.Data),
                { StatusCode: InternalStatusCodes.DbStatus1404NotFound } => NotFound(),
                { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            return Problem();
        }

    }
    
    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Accounts.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TokenResponse>> LoginWithRefreshToken(LoginRefreshTokenRequest request,
        CancellationToken token)
    {
        try
        {
            var validationResult = await _loginRefreshTokenValidator.ValidateAsync(request, token);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await _authenticationService.LoginWithRefreshToken(request, token);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => Ok(result.Data),
                { StatusCode: InternalStatusCodes.AuthStatus3501TokenExpired } => BadRequest(),
                { StatusCode: InternalStatusCodes.DbStatus1404NotFound } => NotFound(),
                { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            return Problem();
        }
    }
    
    [Authorize]
    [MapToApiVersion(1)]
    [HttpDelete(ApiEndpoints.V1.Accounts.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RevokeRefreshTokens([FromRoute] Guid id,
        CancellationToken token)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            if (userId == Guid.Empty) return BadRequest("User not found");
            var result = await _authenticationService.RevokeRefreshTokens(userId, token);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => NoContent(),
                { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            return Problem();
        }
    }
    
    [Authorize]
    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Authentication.Logout)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout(LogoutRefreshTokenRequest request,CancellationToken token)
    {
        try
        {
            var validationResult = await _logoutRefreshTokenValidator.ValidateAsync(request, token);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await _authenticationService.Logout(request, token);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => NoContent(),
                { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            return Problem();
        }
    }

}