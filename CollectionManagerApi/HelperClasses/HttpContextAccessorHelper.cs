using System.Security.Claims;

namespace CollectionManagerApi.HelperClasses;

public class HttpContextAccessorHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextAccessorHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userId = Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var guid) ? guid : Guid.Empty;
        return userId;
    }
}