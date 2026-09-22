using CollectionManagerApi.DTOs.Requests.CollectionItemDocuments;
using CollectionManagerApi.DTOs.Responses;
using CollectionManagerApi.DTOs.Responses.CollectionItemDocuments;

namespace CollectionManagerApi.Services;

public interface ICollectionItemDocumentService
{
    Task<BaseResponse<CollectionItemDocumentsResponse>> Create(Guid userId, Guid collectionItemId, CreateCollectionItemDocumentsRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemDocumentsResponse>> GetById(Guid id, CancellationToken token);
    Task<BaseResponse<List<CollectionItemDocumentsResponse>>> GetAll(Guid userId, Guid collectionItemId, CancellationToken token);
    Task<BaseResponse<CollectionItemDocumentsResponse>> Update(Guid userId, Guid id, UpdateCollectionItemDocumentsRequest request, CancellationToken token);
    Task<BaseResponse<CollectionItemDocumentsResponse>> Delete(Guid userId, Guid id, CancellationToken token);
}