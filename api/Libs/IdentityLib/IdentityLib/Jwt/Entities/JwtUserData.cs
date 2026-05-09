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
    public Guid UserId { get; init; }
    
    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string? Email { get; init; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Claims токена
    /// </summary>
    public IReadOnlyCollection<Claim>? Claims { get; init; }
}