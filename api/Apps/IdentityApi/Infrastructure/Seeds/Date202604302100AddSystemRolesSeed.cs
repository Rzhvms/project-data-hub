using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.RoleSystem;
using FluentMigrator;

namespace Infrastructure.Seeds;

/// <summary>
/// Заполнение таблицы ролей системными значениями по умолчанию.
/// </summary>
[Migration(202604302100)]
public class Date202604302100AddSystemRolesSeed : Migration
{
    private readonly string _roleTableName = EntityMapper.TbName<Role>();

    public override void Up()
    {
        Insert.IntoTable(_roleTableName).Row(new
        {
            Id = Guid.NewGuid(),
            RoleCode = "Viewer",
            Name = "Просмотрщик",
            PermissionsMask = (int)DefaultRolePermissions.Viewer,
            IsSystem = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = (DateTime?)null
        });

        Insert.IntoTable(_roleTableName).Row(new
        {
            Id = Guid.NewGuid(),
            RoleCode = "Editor",
            Name = "Редактор",
            PermissionsMask = (int)DefaultRolePermissions.Editor,
            IsSystem = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = (DateTime?)null
        });

        Insert.IntoTable(_roleTableName).Row(new
        {
            Id = Guid.NewGuid(),
            RoleCode = "Administrator",
            Name = "Администратор",
            PermissionsMask = (int)DefaultRolePermissions.Administrator,
            IsSystem = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = (DateTime?)null
        });
    }

    public override void Down()
    {
        Delete.FromTable(_roleTableName).Row(new { RoleCode = "Administrator" });
        Delete.FromTable(_roleTableName).Row(new { RoleCode = "Editor" });
        Delete.FromTable(_roleTableName).Row(new { RoleCode = "Viewer" });
    }
}