using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CoreLib.User;

/// <summary>
/// Извлекает данные текущего пользователя из JWT-claims через <see cref="IHttpContextAccessor"/>.
/// </summary>
internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    /// <summary>
    /// Идентификатор пользователя из claim <c>ClaimTypes.NameIdentifier</c> (nameid)
    /// </summary>
    public Guid? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return value is not null && Guid.TryParse(value, out var id) ? id : null;
        }
    }

    /// <summary>
    /// Полное имя из claims: <c>GivenName</c> (имя), <c>Surname</c> (фамилия), <c>patronymic</c> (отчество)
    /// </summary>
    public string? UserName
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user is null) return null;

            var name = user.FindFirst(ClaimTypes.GivenName)?.Value;
            var surname = user.FindFirst(ClaimTypes.Surname)?.Value;
            var patronymic = user.FindFirst("patronymic")?.Value;

            var parts = new[] { surname, name, patronymic }.Where(x => !string.IsNullOrWhiteSpace(x));
            return string.Join(" ", parts);
        }
    }
}
