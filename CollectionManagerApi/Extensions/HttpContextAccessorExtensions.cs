using System.Security.Claims;

namespace CollectionManagerApi.Extensions;

public static class HttpContextAccessorExtensions
{
    public static Guid GetUserIdAsGuid(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var guid) ? guid : Guid.Empty;
        return userId;
    }
    public static string GetUserIdAsString(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId;
    }
}