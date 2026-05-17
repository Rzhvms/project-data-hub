using Application.UseCases.Categories;
using Application.UseCases.Categories.Interfaces;
using Application.UseCases.Participants;
using Application.UseCases.Participants.Interfaces;
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
        services.AddScoped<ICategoryUseCaseManager, CategoryUseCaseManager>();
        services.AddScoped<IParticipantUseCaseManager, ParticipantUseCaseManager>();
    }
}