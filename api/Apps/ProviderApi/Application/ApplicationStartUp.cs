using Application.UseCases.ProjectManage;
using Application.UseCases.ProjectManage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Регистрация зависимостей уровня Application
/// </summary>
public static class ApplicationStartUp
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectUseCaseManager, ProjectUseCaseManager>();
    }
}