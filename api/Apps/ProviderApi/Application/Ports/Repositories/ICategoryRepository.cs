using System.Data;
using Domain.Entities.Project.Categories;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для категорий
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Получить все категории
    /// </summary>
    Task<List<ProjectCategory>> GetAllCategories();
    
    /// <summary>
    /// Добавить связь проекта и категории
    /// </summary>
    Task AddProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
   
    /// <summary>
    /// Проверить, есть ли связь проекта и категории
    /// </summary>
    Task<bool> CheckExistProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
   
    /// <summary>
    /// Получить идентификатор категории проекта
    /// </summary>
    Task<Guid> GetCategoryIdByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);
    
    /// <summary>
    /// Получить категорию по идентификатору
    /// </summary>
    Task<ProjectCategory> GetCategoryByIdAsync(Guid categoryId);
    
    /// <summary>
    /// Удалить категорию по идентификатору
    /// </summary>
    Task DeleteCategoryAsync(Guid categoryId);
    
    /// <summary>
    /// Добавить категорию
    /// </summary>
    Task<Guid> AddCategoryAsync(string name, string description);
    
    /// <summary>
    /// Обновить данные категории
    /// </summary>
    Task<ProjectCategory> UpdateCategoryAsync(Guid categoryId, string? name, string? description, bool? isActive = true);
}