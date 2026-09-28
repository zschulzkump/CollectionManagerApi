using Amazon.S3;
using Amazon.S3.Model;
using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses.CollectionItemImages;
using CollectionManagerApi.Extensions;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Mapping;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionItemImagesController : ControllerBase
{
    private readonly ICollectionItemImageService _collectionItemImageService;
    private readonly HttpContextAccessor _httpContextAccessor;
    private readonly IAmazonS3 _s3Client;
    private readonly IValidator<CreateCollectionItemImagesRequest> _createCollectionItemImagesRequestValidator;
    private readonly IValidator<UpdateCollectionItemImagesRequest> _updateCollectionItemImagesRequestValidator;
    
    string? _bucketName = Environment.GetEnvironmentVariable("S3BucketName");
    private const string S3KeyPrefix = "images/";

    public CollectionItemImagesController(
        ICollectionItemImageService collectionItemImageService,
        HttpContextAccessor  httpContextAccessor,
        IAmazonS3 s3Client,
        IValidator<CreateCollectionItemImagesRequest> createCollectionItemImagesRequestValidator,
        IValidator<UpdateCollectionItemImagesRequest> updateCollectionItemImagesRequestValidator)
    {
        _collectionItemImageService = collectionItemImageService;
        _httpContextAccessor = httpContextAccessor;
        _s3Client = s3Client;
        _createCollectionItemImagesRequestValidator = createCollectionItemImagesRequestValidator;
        _updateCollectionItemImagesRequestValidator = updateCollectionItemImagesRequestValidator;
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.CollectionItemImages.Create, Name = "CreateCollectionItemImage")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemImagesResponse>> Create([FromRoute] Guid id, [FromBody] CreateCollectionItemImagesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createCollectionItemImagesRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemImageService.Create(userId: userId, collectionItemId: id, request: request,cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetCollectionItemImageById", new { id = result.Data!.Id }, result.Data),
                {  StatusCode: InternalStatusCodes.Status5500UnknownError } => Problem(),
                _ => BadRequest()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpGet(ApiEndpoints.V1.CollectionItemImages.GetById, Name = "GetCollectionItemImageById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collectionItemImageService.GetById(id, cancellationToken);
            if (result.Data == null) return NotFound();
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
    [HttpGet(ApiEndpoints.V1.CollectionItemImages.GetAll, Name = "GetAllCollectionItemImages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionItemImagesResponse>>> GetAll([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemImageService.GetAll(userId, id, cancellationToken);
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
    [HttpPut(ApiEndpoints.V1.CollectionItemImages.Update, Name = "UpdateCollectionItemImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemImagesResponse>> Update([FromRoute] Guid id,[FromBody] UpdateCollectionItemImagesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateCollectionItemImagesRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemImageService.Update(userId, id, request, cancellationToken);
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
    [HttpDelete(ApiEndpoints.V1.CollectionItemImages.Delete, Name = "DeleteCollectionItemImage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.GetUserIdAsGuid();
            var result = await _collectionItemImageService.Delete(userId, id, cancellationToken);
            if (result.Data == null) return NotFound();
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