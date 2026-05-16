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
    /// Получить полную информацию о проекте по идентификатору
    /// </summary>
    Task<GetFullProjectResponse> GetFullProjectByIdAsync(Guid projectId);
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request);

    /// <summary>
    /// Опубликовать проект. Изменение статуса публикации и удаление черновика
    /// </summary>
    Task PublicateProjectAsync(PublicateProjectRequest request);

    /// <summary>
    /// Обновить информацию о проекте
    /// </summary>
    Task UpdateProjectAsync(Guid projectId, UpdateProjectRequest request);

    /// <summary>
    /// Удалить проект
    /// </summary>
    Task DeleteProjectAsync(Guid projectId);
}