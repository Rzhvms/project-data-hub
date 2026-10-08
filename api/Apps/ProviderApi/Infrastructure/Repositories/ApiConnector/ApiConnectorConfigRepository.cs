using System.Data;
using CoreLib.ApiConnector;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Dapper;
using Domain.Entities.ApiConnector;

namespace Infrastructure.Repositories.ApiConnector;

/// <inheritdoc/>
internal sealed class ApiConnectorConfigRepository(IDbConnection dbConnection) : IApiConnectorConfigRepository
{
    public async Task<List<ApiConnectorConfig>> GetActiveAsync()
    {
        var sql = $"""
            SELECT * FROM {EntityMapper.TbName<ApiConnectorConfigEntity>()}
            WHERE {EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.IsActive)} = true
            """;

        var entities = await dbConnection.QueryAsync<ApiConnectorConfigEntity>(
            new CommandDefinition(sql));

        return entities.Select(e => new ApiConnectorConfig
        {
            Name = e.Name,
            HttpMethod = e.HttpMethod,
            UrlTemplate = e.UrlTemplate,
            HeadersJson = e.HeadersJson,
            BodyTemplate = e.BodyTemplate,
            ContentType = e.ContentType,
            IsActive = e.IsActive
        }).ToList();
    }
}
