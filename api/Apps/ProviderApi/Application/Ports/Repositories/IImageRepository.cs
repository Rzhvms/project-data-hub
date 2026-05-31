using Domain.Entities.Project;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с файлами
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Добавить изображение
    /// </summary>
    Task<Guid> CreateAsync(ProjectImage image);
    
    /// <summary>
    /// Получить изображение по его Id
    /// </summary>
    Task<ProjectImage> GetByIdAsync(Guid imageId);
    
    /// <summary>
    /// Получить все изображения по идентификатору проекта
    /// </summary>
    Task<IEnumerable<ProjectImage>> GetAllByProjectIdAsync(Guid projectId);
    
    // /// <summary>
    // /// Получить главное изображение проекта
    // /// </summary>
    // Task<ProjectImage?> GetMainImageAsync(Guid projectId);
    
    /// <summary>
    /// Обновить изображение 
    /// </summary>
    Task<ProjectImage> UpdateAsync(ProjectImage image);
    
    /// <summary>
    /// Удалить изображение по его Id
    /// </summary>
    Task DeleteAsync(Guid imageId);
}