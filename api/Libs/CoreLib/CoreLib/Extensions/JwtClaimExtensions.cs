using System.Security.Claims;

namespace CoreLib.Extensions;

public static class JwtClaimExtensions
{
    /// <summary>
    /// Метод расширения для получения айди из токена
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(id)) throw new Exception("UserId not found in JWT token");

        return Guid.Parse(id);
    }
}