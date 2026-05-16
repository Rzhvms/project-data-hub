using Application.Ports.Repositories;
using Application.UseCases.Categories.Dto.Response;
using Application.UseCases.Categories.Interfaces;

namespace Application.UseCases.Categories;

internal class CategoryUseCaseManager(ICategoryRepository repository) : ICategoryUseCaseManager
{
    public async Task<GetCategoryListResponse> GetCategoriesAsync()
    {
        var response = await repository.GetAllCategories();
            
        return new GetCategoryListResponse
        {
            CategoryList = response.Select(category => new GetCategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                IsSystem = category.IsSystem,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
            }).ToList()
        };
    }
}