using CoreLib.Audit;
using CoreLib.Database.DapperExtensions.EntityMapper;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Создание таблицы аудита действий пользователей
/// </summary>
[Migration(202606140100)]
public class Date_202606140100_AddAuditLog : Migration
{
    private readonly string _auditLogTb = EntityMapper.TbName<AuditLog>();

    public override void Up()
    {
        Create.Table(_auditLogTb)
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.UserId)).AsGuid().Nullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.UserName)).AsString(255).Nullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.Action)).AsString(255).NotNullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.EntityType)).AsString(255).NotNullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.EntityId)).AsGuid().Nullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.Details)).AsString().Nullable()
            .WithColumn(EntityMapper.ColName<AuditLog>(x => x.CreatedAt)).AsDateTime().NotNullable();

        Create.Index("ix_auditlog_createdat").OnTable(_auditLogTb)
            .OnColumn(EntityMapper.ColName<AuditLog>(x => x.CreatedAt)).Descending();

        Create.Index("ix_auditlog_userid").OnTable(_auditLogTb)
            .OnColumn(EntityMapper.ColName<AuditLog>(x => x.UserId));

        Create.Index("ix_auditlog_entitytype_entityid").OnTable(_auditLogTb)
            .OnColumn(EntityMapper.ColName<AuditLog>(x => x.EntityType)).Ascending()
            .OnColumn(EntityMapper.ColName<AuditLog>(x => x.EntityId)).Ascending();
    }

    public override void Down()
    {
        Delete.Table(_auditLogTb);
    }
}
