using Amazon.S3;
using Amazon.S3.Model;
using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Requests.CollectionItemImages;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.HelperClasses;
using CollectionManagerApi.Models;

namespace CollectionManagerApi.Services;

public class S3Service : IS3Service
{
    private readonly string? _bucketName = Environment.GetEnvironmentVariable("RUSTFS_S3_BucketName");
    private const string ImageKeyPrefix = "images/";
    private const string DocumentKeyPrefix = "documents/";
    private const double PreSignedUrlTimeoutInMinutes = 15;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    
    public async Task<BaseResponse<S3ServiceResult>> UploadItemImage(Guid id, CreateCollectionItemImagesRequest request, CancellationToken token)
    {
        try
        {
            if (_bucketName == null) return new BaseResponse<S3ServiceResult>() {Data = null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
            var extension = Path.GetExtension(request.FileName);
            var originalFilename = Path.GetFileName(request.FileName);
            var s3ImageId = $"{ImageKeyPrefix}{id.ToString()}{extension}";
            var s3Request = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = s3ImageId,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(PreSignedUrlTimeoutInMinutes),
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = originalFilename
                }
            };
            var presignedUrl = await _s3Client.GetPreSignedURLAsync(s3Request);
            return new BaseResponse<S3ServiceResult>() { Data = new S3ServiceResult() {Key = s3ImageId, PreSignedUrl = presignedUrl}, StatusCode = InternalStatusCodes.S3Status2201Created};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }
    }

    public async Task<BaseResponse<S3ServiceResult>> GetItemImageByKey(string key, CancellationToken token)
    {
        try
        {
            var getPreSignedRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15),
            };
            var presignedUrl = await _s3Client.GetPreSignedURLAsync(getPreSignedRequest);
            return new BaseResponse<S3ServiceResult>() { Data =  new S3ServiceResult() {Key = key, PreSignedUrl = presignedUrl}, StatusCode = InternalStatusCodes.S3Status2200Ok};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }

    }

    public async Task<BaseResponse<S3ServiceResult>> DeleteItemImageByKey(string key, CancellationToken token)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = key,
            };
            var deleteResponse = await _s3Client.DeleteObjectAsync(deleteObjectRequest, token);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2200Ok};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }
    }

    public async Task<BaseResponse<S3ServiceResult>> UploadItemDocument(Guid id, CreateCollectionItemDocumentsRequest request, CancellationToken token)
    {
        try
        {
            if (_bucketName == null) return new BaseResponse<S3ServiceResult>() {Data = null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
            var extension = Path.GetExtension(request.FileName);
            var originalFilename = Path.GetFileName(request.FileName);
            var s3ImageId = $"{DocumentKeyPrefix}{id.ToString()}{extension}";
            var s3Request = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = s3ImageId,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(PreSignedUrlTimeoutInMinutes),
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = originalFilename
                }
            };
            var presignedUrl = await _s3Client.GetPreSignedURLAsync(s3Request);
            return new BaseResponse<S3ServiceResult>() { Data = new S3ServiceResult() {Key = s3ImageId, PreSignedUrl = presignedUrl}, StatusCode = InternalStatusCodes.S3Status2201Created};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }
    }

    public async Task<BaseResponse<S3ServiceResult>> GetItemDocumentByKey(string key, CancellationToken token)
    {
        try
        {
            var getPreSignedRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15),
            };
            var presignedUrl = await _s3Client.GetPreSignedURLAsync(getPreSignedRequest);
            return new BaseResponse<S3ServiceResult>() { Data =  new S3ServiceResult() {Key = key, PreSignedUrl = presignedUrl}, StatusCode = InternalStatusCodes.S3Status2200Ok};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }
    }

    public async Task<BaseResponse<S3ServiceResult>> DeleteItemDocumentByKey(string key, CancellationToken token)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = key,
            };
            var deleteResponse = await _s3Client.DeleteObjectAsync(deleteObjectRequest, token);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2200Ok};
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(e);
            return new BaseResponse<S3ServiceResult>() { Data =  null, StatusCode = InternalStatusCodes.S3Status2500ServerError};
        }
    }
}