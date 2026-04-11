using System.ComponentModel.DataAnnotations;

namespace IdentityLib.Jwt.Settings;

/// <summary>
/// Настройки access token.
/// </summary>
public sealed class AccessTokenSettings
{
    /// <summary>
    /// Издатель токена.
    /// </summary>
    [Required]
    public required string Issuer { get; init; }

    /// <summary>
    /// Аудитория токена.
    /// </summary>
    [Required]
    public required string Audience { get; init; }

    /// <summary>
    /// Время жизни токена в секундах.
    /// </summary>
    [Range(1, long.MaxValue)]
    public long LifeTimeInSeconds { get; init; }
}