using System.ComponentModel.DataAnnotations;

namespace IdentityLib.Jwt.Settings;

/// <summary>
/// Настройки refresh token.
/// </summary>
public sealed class RefreshTokenSettings
{
    /// <summary>
    /// Длина сгенерированного токена в байтах.
    /// </summary>
    [Range(16, int.MaxValue)]
    public int Length { get; init; }

    /// <summary>
    /// Время жизни токена в минутах.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int LifeTimeInMinutes { get; init; }
}