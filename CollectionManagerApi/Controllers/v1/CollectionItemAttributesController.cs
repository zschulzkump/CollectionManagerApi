using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemAttribute;
using CollectionManagerApi.DTOs.Responses.CollectionItemAttribute;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionItemAttributesController : ControllerBase
{
    private readonly ICollectionItemAttributeService _collectionItemAttributeService;
    private readonly HttpContextAccessorHelper _httpContextAccessorHelper;
    private readonly IValidator<CreateCollectionItemAttributeRequest> _createCollectionItemAttributeRequest;
    private readonly IValidator<UpdateCollectionItemAttributeRequest> _updateCollectionItemAttributeRequest;

    public CollectionItemAttributesController(
        ICollectionItemAttributeService collectionItemAttributeService,
        HttpContextAccessorHelper httpContextAccessorHelper,
        IValidator<CreateCollectionItemAttributeRequest> createCollectionItemAttributeRequest,
        IValidator<UpdateCollectionItemAttributeRequest> updateCollectionItemAttributeRequest)
    {
        _collectionItemAttributeService = collectionItemAttributeService;
        _httpContextAccessorHelper = httpContextAccessorHelper;
        _createCollectionItemAttributeRequest = createCollectionItemAttributeRequest;
        _updateCollectionItemAttributeRequest = updateCollectionItemAttributeRequest;
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.CollectionItemAttributes.Create, Name = "CreateCollectionItemAttribute")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemAttributeResponse>> Create([FromRoute] Guid id, [FromBody] CreateCollectionItemAttributeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createCollectionItemAttributeRequest.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemAttributeService.Create(id, userId, request, cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetCollectionItemAttributeById", new { id = result.Data!.Id }, result.Data),
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
    [HttpGet(ApiEndpoints.V1.CollectionItemAttributes.GetAll, Name = "GetAllCollectionItemAttributes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionItemAttributeFullResponse>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemAttributeService.GetAll(userId, cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItemAttributes.GetById, Name = "GetCollectionItemAttributeById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemAttributeResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collectionItemAttributeService.GetById(id, cancellationToken);
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
    [HttpPut(ApiEndpoints.V1.CollectionItemAttributes.Update, Name = "UpdateCollectionItemAttribute")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemAttributeResponse>> Update([FromRoute] Guid id,[FromBody] UpdateCollectionItemAttributeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateCollectionItemAttributeRequest.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemAttributeService.Update(userId,id, request, cancellationToken);
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
    [HttpDelete(ApiEndpoints.V1.CollectionItemAttributes.Delete, Name = "DeleteCollectionItemAttribute")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemAttributeService.Delete(userId, id, cancellationToken);
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