using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class AccountController : ControllerBase
{
        private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Accounts.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterV1([FromBody] RegisterUser request, CancellationToken token)
    {
        try
        {
            var result = await _accountService.Register(request, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Created();
        }
        catch (Exception e)
        {
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet(ApiEndpoints.V1.Accounts.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdV1([FromRoute] Guid id, CancellationToken token)
    {
        try
        {
            var response = await _accountService.GetById(id, token);
            if (response.Data == null || !response.Succeeded)
            {
                return NotFound("User not found");
            }
            var user = response.Data;
            return Ok(user.MapToUserResponse());
        }
        catch (Exception e)
        {
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin)]
    [HttpGet(ApiEndpoints.V1.Accounts.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllV1(CancellationToken token)
    {
        try
        {
            var response = await _accountService.GetAll(token);
            if (response.Data == null || !response.Succeeded)
            {
                return NotFound("No users found");
            }
            var users = response.Data;
            var list = users.Select(user => user.MapToUserResponse()).ToList();
            return Ok(list);
        }
        catch (Exception e)
        {
            return Problem();
        }
    }

}