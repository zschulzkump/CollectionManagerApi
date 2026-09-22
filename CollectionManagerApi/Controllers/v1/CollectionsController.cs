using System.Security.Claims;
using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionGroup;
using CollectionManagerApi.DTOs.Responses.Collections;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionsController: ControllerBase
{
    private readonly ICollectionGroupService _collectionGroupService;
    private readonly IValidator<CreateCollectionGroupRequest> _createCollectionGroupRequestValidator;
    private readonly IValidator<UpdateCollectionGroupRequest> _updateCollectionGroupRequestValidator;
    private readonly HttpContextAccessorHelper _httpContextAccessorHelper;

    public CollectionsController(ICollectionGroupService collectionGroupService,
        IValidator<CreateCollectionGroupRequest> createCollectionGroupRequestValidator,
        IValidator<UpdateCollectionGroupRequest> updateCollectionGroupRequestValidator,
        HttpContextAccessorHelper httpContextAccessorHelper)
    {
        _collectionGroupService = collectionGroupService;
        _createCollectionGroupRequestValidator = createCollectionGroupRequestValidator;
        _updateCollectionGroupRequestValidator = updateCollectionGroupRequestValidator;
        _httpContextAccessorHelper = httpContextAccessorHelper;
    }

    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.Collections.Create, Name = "CreateCollection")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionGroupResponse>> Create([FromBody] CreateCollectionGroupRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createCollectionGroupRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionGroupService.Create(userId, request, cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetCollectionById", new { id = result.Data!.Id }, result.Data),
                {  StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet(ApiEndpoints.V1.Collections.GetAll, Name = "GetAllCollections")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionGroupResponse>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionGroupService.GetAll(userId, cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1200Ok } => Ok(result.Data),
                {  StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet(ApiEndpoints.V1.Collections.GetById, Name = "GetCollectionById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionGroupResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collectionGroupService.GetById(id, cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1200Ok } => Ok(result.Data),
                {  StatusCode: InternalStatusCodes.DbStatus1404NotFound } => NotFound(),
                {  StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPut(ApiEndpoints.V1.Collections.Update, Name = "UpdateCollection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionGroupResponse>> Update([FromRoute] Guid id,[FromBody] UpdateCollectionGroupRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateCollectionGroupRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionGroupService.Update(userId, id, request, cancellationToken);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => Ok(result.Data),
                { StatusCode: InternalStatusCodes.DbStatus1404NotFound } => NotFound(),
                { StatusCode: InternalStatusCodes.DbStatus1502UpdateError} or { StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpDelete(ApiEndpoints.V1.Collections.Delete, Name = "DeleteCollection")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionGroupService.Delete(userId, id, cancellationToken);
            return result switch
            {
                { StatusCode: InternalStatusCodes.DbStatus1200Ok } => NoContent(),
                { StatusCode: InternalStatusCodes.DbStatus1404NotFound } => NotFound(),
                { StatusCode: InternalStatusCodes.DbStatus1503DeleteError } or {StatusCode: InternalStatusCodes.Status5500UnknownError} => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }
}