using CoreLib.Database.DapperExtensions.TypeHandlers;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.Database;

public static class CoreDatabases
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddCoreDatabases(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new JsonObjectTypeHandler());
    }
}