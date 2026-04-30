using System.Data;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.IdentityUser;
using Domain.Entities.RoleSystem;
using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(202604290210)]
public class Date_202604290210_AddIdentityTables : Migration
{
    private readonly string _userTableName = EntityMapper.TbName<User>();
    private readonly string _roleTableName = EntityMapper.TbName<Role>();
    private readonly string _refreshTokenTableName = EntityMapper.TbName<RefreshToken>();
    
    public override void Up()
    {
        Create.Table(_roleTableName)
            .WithColumn(EntityMapper.ColName<Role>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<Role>(x => x.RoleCode)).AsString(50).NotNullable()
            .WithColumn(EntityMapper.ColName<Role>(x => x.Name)).AsString(100).NotNullable()
            .WithColumn(EntityMapper.ColName<Role>(x => x.PermissionsMask)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(EntityMapper.ColName<Role>(x => x.IsSystem)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<Role>(x => x.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(EntityMapper.ColName<Role>(x => x.UpdatedAt)).AsDateTime().Nullable();
            
        Create.UniqueConstraint("uq_roles_role_code")
            .OnTable(_roleTableName)
            .Column(EntityMapper.ColName<Role>(x => x.RoleCode));
        
        Create.Table(_userTableName)
            .WithColumn(EntityMapper.ColName<User>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.Username)).AsString(50).NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.Email)).AsString(50).NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.Phone)).AsString(50).Nullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.Password)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.HashSalt)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.IsEmailConfirmed)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(EntityMapper.ColName<User>(x => x.FirstName)).AsString(50).Nullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.LastName)).AsString(50).Nullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.RoleId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<User>(x => x.CreatedAt)).AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn(EntityMapper.ColName<User>(x => x.UpdatedAt)).AsDateTime().Nullable();
            
        Create.UniqueConstraint("uq_users_username")
            .OnTable(_userTableName)
            .Column(EntityMapper.ColName<User>(x => x.Username));
            
        Create.UniqueConstraint("uq_users_email")
            .OnTable(_userTableName)
            .Column(EntityMapper.ColName<User>(x => x.Email));
        
        Create.ForeignKey("fk_users_roles_role_id")
            .FromTable(_userTableName).ForeignColumn(EntityMapper.ColName<User>(x => x.RoleId))
            .ToTable(_roleTableName).PrimaryColumn(EntityMapper.ColName<Role>(x => x.Id));
        
        Create.Table(_refreshTokenTableName)
            .WithColumn(EntityMapper.ColName<RefreshToken>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<RefreshToken>(x => x.UserId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<RefreshToken>(x => x.Value)).AsString(512).Nullable()
            .WithColumn(EntityMapper.ColName<RefreshToken>(x => x.Active)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<RefreshToken>(x => x.ExpirationDate)).AsDateTime().NotNullable();
        
        Create.ForeignKey("fk_refresh_tokens_users_user_id")
            .FromTable(_refreshTokenTableName).ForeignColumn(EntityMapper.ColName<RefreshToken>(x => x.UserId))
            .ToTable(_userTableName).PrimaryColumn(EntityMapper.ColName<User>(x => x.Id))
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    { 
        Delete.Table(_refreshTokenTableName); 
        Delete.Table(_userTableName);
        Delete.Table(_roleTableName);        
    }
}