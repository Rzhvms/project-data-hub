using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.IdentityUser;
using FluentMigrator;
using IdentityLib.Encryption.Interfaces;

namespace Infrastructure.Seeds;

/// <summary>
/// Создание тестового пользователя
/// </summary>
[Migration(202605100200)]
public class AddDefaultUserSeed(IPasswordEncryptionService encryptionService, IRoleRepository roleRepository) : Migration
{
    private readonly string _userTableName = EntityMapper.TbName<User>();
    private readonly string _systemUserEmail = "system_user@inpad.ru";
    private readonly string _systemPassword = "system_!83q2;user";
    
    public override void Up()
    {
        var salt = encryptionService.GenerateSalt();
        var role = roleRepository.GetRoleByRoleCode("Administrator").Result;
        
        Insert.IntoTable(_userTableName).Row(new
        {
            Id = Guid.NewGuid(),
            Email = _systemUserEmail,
            Password = encryptionService.HashPassword(_systemPassword, salt),
            HashSalt = Convert.ToBase64String(salt),
            IsEmailConfirmed = true,
            RoleId = role.Id,
            FirstName = "System",
            LastName = "Administrator",
            CreatedAt = DateTime.UtcNow
        });
    }

    public override void Down() { }
}