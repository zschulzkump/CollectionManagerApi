using System.Security.Claims;
using Asp.Versioning;
using Collection_Manager_Api.DTOs.Requests;
using Collection_Manager_Api.HelperClasses;
using Collection_Manager_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collection_Manager_Api.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthenticationController(IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor)
    {
        _authenticationService = authenticationService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Authentication.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginV1([FromBody] LoginRequest request, CancellationToken token)
    {
        try
        {
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
            var ipV4Address = _httpContextAccessor.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            var ipV6Address = _httpContextAccessor.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv6().ToString();
            var httpRequestMeta = new HttpRequestMeta()
            {
                UserAgent = userAgent,
                IpV4Address = ipV4Address,
                IpV6Address = ipV6Address
            };
            var result = await _authenticationService.Login(request, httpRequestMeta, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
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
    public async Task<IActionResult> LoginWithRefreshTokenV1(LoginRefreshTokenRequest request,
        CancellationToken token)
    {
        try
        {
            var result = await _authenticationService.LoginWithRefreshToken(request, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
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
    public async Task<IActionResult> RevokeRefreshTokensV1([FromRoute] Guid id,
        CancellationToken token)
    {
        try
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
        
            if (userId == Guid.Empty)
            {
                return BadRequest("User not found");
            }
        
            await _authenticationService.RevokeRefreshTokens(userId, token);
        }
        catch (Exception e)
        {
            return Problem();
        }
        
        return NoContent();
    }
    [Authorize]
    [MapToApiVersion(1)]
    [HttpGet(ApiEndpoints.V1.Accounts.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRefreshTokensV1([FromRoute] Guid id,
        CancellationToken token)
    {
        try
        {
            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
        
            if (userId == Guid.Empty)
            {
                return BadRequest("User not found");
            }
        
            var response = await _authenticationService.GetRefreshTokens(userId, token);
            return Ok(response);
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
    public async Task<IActionResult> LogoutV1(LogoutRefreshTokenRequest request,CancellationToken token)
    {
        try
        {
            var result = await _authenticationService.Logout(request, token);
            if (!result)
            {
                return BadRequest("Invalid refresh token");
            }
            return NoContent();
        }
        catch (Exception e)
        {
            return Problem();
        }
    }

}