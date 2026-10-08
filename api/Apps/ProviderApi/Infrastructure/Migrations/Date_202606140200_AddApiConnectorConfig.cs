using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.ApiConnector;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Создание таблицы конфигураций Api-коннекторов
/// </summary>
[Migration(202606140200)]
public class Date_202606140200_AddApiConnectorConfig : Migration
{
    private readonly string _connectorTb = EntityMapper.TbName<ApiConnectorConfigEntity>();

    public override void Up()
    {
        Create.Table(_connectorTb)
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.Name)).AsString(255).NotNullable().Unique()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.HttpMethod)).AsString(10).NotNullable()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.UrlTemplate)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.HeadersJson)).AsString().NotNullable().WithDefaultValue("{}")
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.BodyTemplate)).AsString().Nullable()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.ContentType)).AsString(100).NotNullable().WithDefaultValue("application/json")
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(EntityMapper.ColName<ApiConnectorConfigEntity>(x => x.UpdatedAt)).AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Table(_connectorTb);
    }
}
