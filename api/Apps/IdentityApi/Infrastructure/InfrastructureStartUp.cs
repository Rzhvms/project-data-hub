using System.Data;
using Application.Ports.Repositories;
using FluentMigrator.Runner;
using Infrastructure.Migrations;
using Infrastructure.Repositories.RoleSystem;
using Infrastructure.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using UserRepository = Infrastructure.Repositories.Users.UserRepository;

namespace Infrastructure;

/// <summary>
/// Регистрация зависимостей уровня Infrastructure
/// </summary>
public static class InfrastructureStartUp
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Postgres
        services.AddScoped<IDbConnection>(_ =>
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        });
        
        // FluentMigrator
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Date_202604290210_AddIdentityTables).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}