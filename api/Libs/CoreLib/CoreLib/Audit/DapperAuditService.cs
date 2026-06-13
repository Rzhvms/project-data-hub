using System.Data;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Dapper;

namespace CoreLib.Audit;

/// <inheritdoc/>
internal sealed class DapperAuditService(IDbConnection connection) : IAuditService
{
    /// <inheritdoc/>
    public async Task LogAsync(string action, string entityType, Guid? entityId = null,
        Guid? userId = null, string? userName = null, string? details = null)
    {
        var sql = $"""
            INSERT INTO {EntityMapper.TbName<AuditLog>()}
                ({EntityMapper.ColName<AuditLog>(x => x.Id)},
                 {EntityMapper.ColName<AuditLog>(x => x.UserId)},
                 {EntityMapper.ColName<AuditLog>(x => x.UserName)},
                 {EntityMapper.ColName<AuditLog>(x => x.Action)},
                 {EntityMapper.ColName<AuditLog>(x => x.EntityType)},
                 {EntityMapper.ColName<AuditLog>(x => x.EntityId)},
                 {EntityMapper.ColName<AuditLog>(x => x.Details)},
                 {EntityMapper.ColName<AuditLog>(x => x.CreatedAt)})
            VALUES (@Id, @UserId, @UserName, @Action, @EntityType, @EntityId, @Details, @CreatedAt)
            """;

        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = userName,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        }));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AuditLog>> GetLogsAsync(int? limit = null)
    {
        var sql = $"""
            SELECT * FROM {EntityMapper.TbName<AuditLog>()}
            ORDER BY {EntityMapper.ColName<AuditLog>(x => x.CreatedAt)} DESC
            """;

        if (limit.HasValue) sql += $"\nLIMIT {limit.Value}";

        return await connection.QueryAsync<AuditLog>(new CommandDefinition(sql));
    }
}
