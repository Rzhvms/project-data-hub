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
    Task<Guid> CreateProjectAsync(ProjectCard request);
}