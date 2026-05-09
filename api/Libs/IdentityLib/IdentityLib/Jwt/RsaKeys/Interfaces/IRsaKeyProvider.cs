using Microsoft.IdentityModel.Tokens;

namespace IdentityLib.Jwt.RsaKeys.Interfaces;

/// <summary>
/// RSA-ключи для подписи и проверки JWT.
/// При первом запуске создает директорию и ключи, если их нет.
/// </summary>
public interface IRsaKeyProvider : IDisposable
{
    /// <summary>
    /// Ключ для подписи токенов.
    /// </summary>
    RsaSecurityKey SigningKey { get; }

    /// <summary>
    /// Ключ для проверки токенов.
    /// </summary>
    RsaSecurityKey ValidationKey { get; }
}