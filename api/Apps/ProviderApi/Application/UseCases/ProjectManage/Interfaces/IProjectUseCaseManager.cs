using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;

namespace Application.UseCases.ProjectManage.Interfaces;

/// <summary>
/// UseCase сценарии по управлению проектами
/// </summary>
public interface IProjectUseCaseManager
{
    /// <summary>
    /// Создание проекта
    /// </summary>
    Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request);
}