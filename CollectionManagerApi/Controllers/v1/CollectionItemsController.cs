using System.Security.Claims;
using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItem;
using CollectionManagerApi.DTOs.Responses.CollectionItems;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.Services;
using CollectionManagerApi.HelperClasses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionItemsController : ControllerBase
{
    private readonly ICollectionItemService _collectionItemService;
    private readonly IValidator<CreateCollectionItemRequest> _createCollectionItemValidator;
    private readonly IValidator<UpdateCollectionItemRequest> _updateCollectionItemValidator;
    private readonly HttpContextAccessor _httpContextAccessor;

    public CollectionItemsController(
        ICollectionItemService collectionItemService,
        IValidator<CreateCollectionItemRequest> collectionItemValidator,
        IValidator<UpdateCollectionItemRequest> updateCollectionItemValidator,
        HttpContextAccessor httpContextAccessor)
    {
        _collectionItemService = collectionItemService;
        _createCollectionItemValidator = collectionItemValidator;
        _updateCollectionItemValidator = updateCollectionItemValidator;
        _httpContextAccessor =  httpContextAccessor;
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.CollectionItems.Create, Name = "CreateCollectionItem")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemResponse>> Create([FromRoute] Guid id, [FromBody] CreateCollectionItemRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createCollectionItemValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemService.Create(userId, id, request, cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItems.GetAllByCollectionId, Name = "GetAllCollectionItemsByCollectionId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionItemResponse>>> GetAllByCollectionId([FromRoute] Guid id,CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemService.GetAllByCollectionId(userId, id, cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItems.GetAll, Name = "GetAllCollectionItems")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionItemResponse>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemService.GetAll(userId, cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItems.GetById, Name = "GetCollectionItemById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collectionItemService.GetById(id, cancellationToken);
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
    [HttpPut(ApiEndpoints.V1.CollectionItems.Update, Name = "UpdateCollectionItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemResponse>> Update([FromRoute] Guid id,[FromBody] UpdateCollectionItemRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateCollectionItemValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemService.Update(userId, id, request, cancellationToken);
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
    [HttpDelete(ApiEndpoints.V1.CollectionItems.Delete, Name = "DeleteCollectionItem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemService.Delete(userId, id, cancellationToken);
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