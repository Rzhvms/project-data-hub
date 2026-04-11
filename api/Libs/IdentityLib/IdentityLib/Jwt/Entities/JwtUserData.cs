using System.Security.Claims;

namespace IdentityLib.Jwt.Entities;

/// <summary>
/// Минимальный набор данных о пользователе, необходимый для выпуска JWT.
/// </summary>
public sealed record JwtUserData
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор (имя) пользователя
    /// </summary>
    public string? UserName { get; set; } = null;
    
    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string? Email { get; set; } = null;
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? FirstName { get; set; } = null;
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string? LastName { get; set; } = null;
    
    /// <summary>
    /// Claims токена
    /// </summary>
    public IReadOnlyCollection<Claim>? Claims { get; set; } = null;
}