namespace CollectionManagerApi.HelperClasses;

public static class InternalStatusCodes
{
    public const int DbStatus1200Ok = 1200;
    public const int DbStatus1201Created = 1201;
    public const int DbStatus1404NotFound = 1404;
    public const int DbStatus1501InsertError = 1501;
    public const int DbStatus1502UpdateError = 1502;
    public const int DbStatus1503DeleteError = 1503;
    
    public const int S3Status2200Ok = 2200;
    public const int S3Status2201Created = 2201;
    public const int S3Status2404NotFound = 2404;
    public const int S3Status2500ServerError = 2500;
    public const int S3Status2501InsertError = 2501;
    public const int S3Status2502UpdateError = 2502;
    public const int S3Status2503DeleteError = 2503;
    
    public const int AuthStatus3501TokenExpired = 3501;
    
    
    public const int Status5500UnknownError = 5500;
}
