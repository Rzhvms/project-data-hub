using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.Audit;

/// <summary>
/// Регистрация сервисов аудита в DI
/// </summary>
public static class AuditStartUp
{
    public static void AddCoreAudit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuditService, DapperAuditService>();
        services.Configure<AuditLogCleanupOptions>(configuration.GetSection("AuditLogCleanup"));
        services.AddHostedService<AuditLogCleanupHostedService>();
    }
}
