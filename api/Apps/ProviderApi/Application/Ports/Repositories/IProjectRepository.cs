using System.Data;
using Domain.Entities.Project;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для обработки данных проектов
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Получение списка объектов для главной страницы
    /// </summary>
    Task<List<ProjectCard>> GetSimpleProjectListAsync();
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    Task<Guid> CreateProjectAsync(ProjectCard request, ProjectDraftData draftData);

    /// <summary>
    /// Опубликовать проект
    /// </summary>
    Task PublishProjectAsync(Guid projectId, ProjectDraftData draftData, IDbTransaction? transaction = null);

    /// <summary>
    /// Получить черновик
    /// </summary>
    Task<ProjectCardDraft?> GetDraftByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);

    /// <summary>
    /// Обновить статус публикации
    /// </summary>
    Task UpdatePublicationStatusAsync(Guid projectId, ProjectPublicationStatus status, IDbTransaction? transaction = null);

    /// <summary>
    /// Удалить черновик
    /// </summary>
    Task RemoveDraftByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    IDbTransaction BeginTransaction();
}