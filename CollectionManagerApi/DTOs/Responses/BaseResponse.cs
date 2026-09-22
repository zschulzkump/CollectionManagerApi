namespace CollectionManagerApi.DTOs.Responses;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Errors { get; set; }
    public string Message { get; set; } =  string.Empty;
}