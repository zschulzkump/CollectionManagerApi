namespace CollectionManagerApi.HelperClasses;

public class FileManager
{
    public Task<string> CreateNewFileName(Guid imageId, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var newFilename = $"{imageId.ToString()}{extension}";
        return Task.FromResult(newFilename);
    }
}