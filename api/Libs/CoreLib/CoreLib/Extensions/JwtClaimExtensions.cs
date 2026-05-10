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
    
    /// <summary>
    /// Метод расширения для получения айди из токена
    /// </summary>
    public static string GetUserFio(this ClaimsPrincipal user)
    {
        var name = user.FindFirst(ClaimTypes.GivenName)?.Value;
        var surname = user.FindFirst(ClaimTypes.Surname)?.Value;
        var patronymic = user.FindFirst("patronymic")?.Value;
        
        var list = new List<string>(3) { name!, surname!, patronymic! }
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
        
        return string.Join(" ", list);
    }
}