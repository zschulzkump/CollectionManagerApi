using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests;
using CollectionManagerApi.DTOs.Requests.Account;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.Account;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IValidator<RegisterUser> _validator;
    public AccountController(IAccountService accountService, IValidator<RegisterUser> validator)
    {
        _accountService = accountService;
        _validator = validator;
    }

    [MapToApiVersion(1)]
    [HttpPost(ApiEndpoints.V1.Accounts.Create, Name = "RegisterUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> RegisterV1([FromBody] RegisterUser request, CancellationToken token)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, token);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await _accountService.Register(request, token);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetUserById", new { id = result.Data!.UserId }, result.Data),
                { StatusCode: InternalStatusCodes.DbStatus1501InsertError } or { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet(ApiEndpoints.V1.Accounts.GetById, Name = "GetUserById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> GetUserById([FromRoute] Guid id, CancellationToken token)
    {
        try
        {
            var result = await _accountService.GetById(id, token);
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
    [Authorize(Roles = Roles.Admin)]
    [HttpGet(ApiEndpoints.V1.Accounts.GetAll, Name = "GetAllUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllV1(CancellationToken token)
    {
        try
        {
            var result = await _accountService.GetAll(token);
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

}