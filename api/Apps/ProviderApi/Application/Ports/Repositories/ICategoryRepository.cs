using System.Data;
using Domain.Entities.Project.Categories;

namespace Application.Ports.Repositories;

public interface ICategoryRepository
{
    Task<List<ProjectCategory>> GetAllCategories();
    Task AddProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
    Task<bool> CheckExistProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null);
    Task<Guid> GetCategoryIdByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);
}