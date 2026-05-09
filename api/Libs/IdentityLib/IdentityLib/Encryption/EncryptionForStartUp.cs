using IdentityLib.Encryption.Interfaces;
using IdentityLib.Encryption.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityLib.Encryption;

public static class EncryptionForStartUp
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddIdentityEncryption(this IServiceCollection services)
    {
        services.AddScoped<IPasswordEncryptionService, PasswordEncryptionService>();
    }
}