using System.Data;
using FluentMigrator.Runner;
using Infrastructure.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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
        services.AddScoped<IDbConnection>(sp =>
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
    }
}