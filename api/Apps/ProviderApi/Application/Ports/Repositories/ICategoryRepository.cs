using System.Data;
using Domain.Entities.Project.Categories;

namespace Application.Ports.Repositories;

public interface ICategoryRepository
{
    Task<List<ProjectCategory>> GetAllCategories();
    Task AddProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
    Task<bool> CheckExistProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
    Task<Guid> GetCategoryIdByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);
    Task<ProjectCategory> GetCategoryByIdAsync(Guid categoryId);
    Task DeleteCategoryAsync(Guid categoryId);
    Task<Guid> AddCategoryAsync(string name, string description);
    Task<ProjectCategory> UpdateCategoryAsync(Guid categoryId, string? name, string? description, bool? isActive = true);
}