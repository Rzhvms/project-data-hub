using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;

namespace Application.UseCases.ProjectManage.Interfaces;

/// <summary>
/// UseCase сценарии по управлению проектами
/// </summary>
public interface IProjectUseCaseManager
{
    /// <summary>
    /// Получение списка объектов для главной страницы
    /// </summary>
    Task<GetSimpleProjectListResponse> GetSimpleProjectListAsync();
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request);

    /// <summary>
    /// Опубликовать проект. Изменение статуса публикации и удаление черновика
    /// </summary>
    Task PublicateProjectAsync(PublicateProjectRequest request);
}