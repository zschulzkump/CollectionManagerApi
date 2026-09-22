using Amazon.S3;
using Asp.Versioning;
using CollectionManagerApi.Database;
using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Responses.CollectionItemDocuments;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectionManagerApi.Controllers.v1;

[ApiController]
[ApiVersion(1)]
public class CollectionItemDocumentsController : ControllerBase
{
    private readonly ICollectionItemDocumentService _collectionItemDocumentService;
    private readonly HttpContextAccessorHelper _httpContextAccessorHelper;
    private readonly IAmazonS3 _s3Client;
    private readonly IValidator<CreateCollectionItemDocumentsRequest> _createCollectionItemDocumetsRequestValidator;
    private readonly IValidator<UpdateCollectionItemDocumentsRequest> _updateCollectionItemDocumetsRequestValidator;
    
    string? _bucketName = Environment.GetEnvironmentVariable("S3BucketName");
    private const string S3KeyPrefix = "images/";

    public CollectionItemDocumentsController(
        ICollectionItemDocumentService collectionItemDocumentService,
        HttpContextAccessorHelper httpContextAccessorHelper,
        IAmazonS3 s3Client,
        IValidator<CreateCollectionItemDocumentsRequest> createCollectionItemDocumetsRequestValidator,
        IValidator<UpdateCollectionItemDocumentsRequest> updateCollectionItemDocumetsRequestValidator)
    {
        _collectionItemDocumentService = collectionItemDocumentService;
        _httpContextAccessorHelper = httpContextAccessorHelper;
        _s3Client = s3Client;
        _createCollectionItemDocumetsRequestValidator = createCollectionItemDocumetsRequestValidator;
        _updateCollectionItemDocumetsRequestValidator = updateCollectionItemDocumetsRequestValidator;
    }
    
    [MapToApiVersion(1)]
    [Authorize(Roles = Roles.Admin + "," + Roles.User)]
    [HttpPost(ApiEndpoints.V1.CollectionItemDocuments.Create, Name = "CreateCollectionItemDocument")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemDocumentsResponse>> Create([FromRoute] Guid id, [FromBody] CreateCollectionItemDocumentsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createCollectionItemDocumetsRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemDocumentService.Create(userId: userId, collectionItemId: id, request: request,cancellationToken);
            return result switch
            {
                {  StatusCode: InternalStatusCodes.DbStatus1201Created } => CreatedAtRoute("GetCollectionItemDocumentById", new { id = result.Data!.Id }, result.Data),
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
    [HttpGet(ApiEndpoints.V1.CollectionItemDocuments.GetById, Name = "GetCollectionItemDocumentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemDocumentsResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _collectionItemDocumentService.GetById(id, cancellationToken);
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
    [HttpGet(ApiEndpoints.V1.CollectionItemDocuments.GetAll, Name = "GetAllCollectionItemDocuments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CollectionItemDocumentsResponse>>> GetAll([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemDocumentService.GetAll(userId, id, cancellationToken);
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
    [HttpPut(ApiEndpoints.V1.CollectionItemDocuments.Update, Name = "UpdateCollectionItemDocument")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CollectionItemDocumentsResponse>> Update([FromRoute] Guid id,[FromBody] UpdateCollectionItemDocumentsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateCollectionItemDocumetsRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemDocumentService.Update(userId, id, request, cancellationToken);
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
    [HttpDelete(ApiEndpoints.V1.CollectionItemDocuments.Delete, Name = "DeleteCollectionItemDocument")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessorHelper.GetUserId();
            var result = await _collectionItemDocumentService.Delete(userId, id, cancellationToken);
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