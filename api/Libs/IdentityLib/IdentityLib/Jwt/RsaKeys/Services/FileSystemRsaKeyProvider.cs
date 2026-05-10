using System.Security.Cryptography;
using IdentityLib.Jwt.RsaKeys.Interfaces;
using IdentityLib.Jwt.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityLib.Jwt.RsaKeys.Services;

/// <summary>
/// Файловая реализация провайдера RSA-ключей.
/// </summary>
public sealed class FileSystemRsaKeyProvider : IRsaKeyProvider
{
    private readonly RSA _signingRsa;
    private readonly RSA _validationRsa;

    public RsaSecurityKey SigningKey { get; }
    public RsaSecurityKey ValidationKey { get; }

    public FileSystemRsaKeyProvider(IOptions<JwtKeyStoreSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        var settings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));

        var directoryPath = Path.IsPathFullyQualified(settings.DirectoryPath)
            ? settings.DirectoryPath
            : Path.Combine(AppContext.BaseDirectory, settings.DirectoryPath);
        
        Directory.CreateDirectory(directoryPath);

        var privateKeyPath = Path.Combine(directoryPath, settings.PrivateKeyFileName);
        var publicKeyPath = Path.Combine(directoryPath, settings.PublicKeyFileName);

        if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
        {
            GenerateAndStoreKeys(
                privateKeyPath,
                publicKeyPath,
                settings.RsaKeySize);
        }

        _signingRsa = RSA.Create();
        _signingRsa.ImportFromPem(File.ReadAllText(privateKeyPath));

        _validationRsa = RSA.Create();
        _validationRsa.ImportFromPem(File.ReadAllText(publicKeyPath));

        SigningKey = new RsaSecurityKey(_signingRsa);
        ValidationKey = new RsaSecurityKey(_validationRsa);
    }

    /// <summary>
    /// Генерация и сохранение RSA-ключей.
    /// </summary>
    private static void GenerateAndStoreKeys(string privateKeyPath, string publicKeyPath, int rsaKeySize)
    {
        using var rsa = RSA.Create(rsaKeySize);

        var privatePem = rsa.ExportPkcs8PrivateKeyPem();
        var publicPem = rsa.ExportSubjectPublicKeyInfoPem();

        File.WriteAllText(privateKeyPath, privatePem);
        File.WriteAllText(publicKeyPath, publicPem);
    }

    /// <inheritdoc /> 
    public void Dispose()
    {
        _signingRsa.Dispose();
        _validationRsa.Dispose();
    }
}