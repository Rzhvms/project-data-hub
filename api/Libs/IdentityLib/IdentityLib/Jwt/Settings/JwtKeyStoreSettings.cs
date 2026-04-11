using System.ComponentModel.DataAnnotations;

namespace IdentityLib.Jwt.Settings;

/// <summary>
/// Настройки хранения RSA-ключей.
/// </summary>
public sealed class JwtKeyStoreSettings
{
    /// <summary>
    /// Адрес хранения ключей в директории проекта.
    /// </summary>
    [Required]
    public required string DirectoryPath { get; init; }

    /// <summary>
    /// Название файла приватного ключа.
    /// </summary>
    public string PrivateKeyFileName { get; init; } = "jwt-private.pem";

    /// <summary>
    /// Название файла публичного ключа.
    /// </summary>
    public string PublicKeyFileName { get; init; } = "jwt-public.pem";

    /// <summary>
    /// Размер RSA-ключа в байтах.
    /// </summary>
    [Range(2048, 8192)]
    public int RsaKeySize { get; init; } = 4096;
}