using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.Tags;
using CollectionManagerApi.DTOs.Responses.Tags;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionItemTagsController : ControllerBase
{
    private readonly ITagsService _tagsService;
    private readonly IValidator<CreateTagRequest> _createTagRequestValidator;
    private readonly IValidator<UpdateTagRequest> _updateTagRequestValidator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CollectionItemTagsController(
        ITagsService tagsService,
        IValidator<CreateTagRequest> createTagRequestValidator,
        IValidator<UpdateTagRequest> updateTagRequestValidator,
        IHttpContextAccessor httpContextAccessor){
        _tagsService = tagsService;
        _createTagRequestValidator = createTagRequestValidator;
        _updateTagRequestValidator = updateTagRequestValidator;
        _httpContextAccessor = httpContextAccessor;
    }
    
     [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.CollectionItemTags.Create, Name = "CreateCollectionItemTag")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TagsResponse>> Create([FromRoute] Guid id, [FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createTagRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _tagsService.Create(userId, id, request, cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetCollectionItemTagById", new { id = result.Data!.Id }, result.Data),
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
    [HttpGet(ApiEndpoints.V1.CollectionItemTags.GetAllByCollectionId, Name = "GetAllCollectionItemTags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TagsResponse>>> GetAllByCollectionId([FromRoute] Guid id,CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _tagsService.GetAllByCollectionId(userId, id,cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItemTags.GetAll, Name = "GetAllTags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<TagsResponse>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _tagsService.GetAll(userId,cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItemTags.GetById, Name = "GetCollectionItemTagById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TagsResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tagsService.GetById(id, cancellationToken);
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
    [HttpPut(ApiEndpoints.V1.CollectionItemTags.Update, Name = "UpdateCollectionItemTag")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TagsResponse>> Update([FromRoute] Guid id,[FromBody] UpdateTagRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateTagRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _tagsService.Update(userId, id, request, cancellationToken);
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
    [HttpDelete(ApiEndpoints.V1.CollectionItemTags.Delete, Name = "DeleteCollectionItemTag")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _tagsService.Delete(userId, id, cancellationToken);
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