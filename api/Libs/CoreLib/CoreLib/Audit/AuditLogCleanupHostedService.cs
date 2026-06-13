using System.Data;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreLib.Audit;

/// <summary>
/// Фоновый сервис для автоматического удаления записей аудита старше заданного периода
/// </summary>
internal sealed class AuditLogCleanupHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<AuditLogCleanupOptions> options,
    ILogger<AuditLogCleanupHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AuditLog cleanup started (retention: {Days} days, check every: {Hours}h)",
            options.Value.RetentionDays, options.Value.CheckIntervalHours);

        var interval = TimeSpan.FromHours(options.Value.CheckIntervalHours);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "AuditLog cleanup failed");
            }

            await Task.Delay(interval, stoppingToken);
        }
    }

    private async Task CleanupAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        var threshold = DateTime.UtcNow.AddDays(-options.Value.RetentionDays);

        var sql = $"""
            DELETE FROM {EntityMapper.TbName<AuditLog>()}
            WHERE {EntityMapper.ColName<AuditLog>(x => x.CreatedAt)} < @Threshold
            """;

        var deleted = await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            Threshold = threshold
        }, cancellationToken: ct));

        if (deleted > 0)
        {
            logger.LogInformation("Deleted {Count} audit log entries older than {Days} days", deleted, options.Value.RetentionDays);
        }
    }
}
