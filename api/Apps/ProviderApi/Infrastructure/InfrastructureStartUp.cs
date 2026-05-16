using System.Data;
using Application.Ports.Repositories;
using FluentMigrator.Runner;
using Infrastructure.Migrations;
using Infrastructure.Repositories.Categories;
using Infrastructure.Repositories.Project;
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
                .ScanIn(typeof(Date_202605012350_AddProjectTables).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}