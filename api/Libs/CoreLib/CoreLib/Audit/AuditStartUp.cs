using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.Audit;

/// <summary>
/// Регистрация сервисов аудита в DI-контейнере
/// </summary>
public static class AuditStartUp
{
    /// <summary>
    /// Регистрирует <see cref="IAuditService"/> как scoped-сервис
    /// </summary>
    public static void AddCoreAudit(this IServiceCollection services)
    {
        services.AddScoped<IAuditService, DapperAuditService>();
    }
}
