using IdentityLib.Encryption;
using IdentityLib.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityLib;

public static class IdentityStartUp
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddIdentityLib(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityJwt(configuration);
        services.AddIdentityEncryption();
    }
}