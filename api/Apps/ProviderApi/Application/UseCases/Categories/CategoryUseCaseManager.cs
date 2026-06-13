using Application.Ports.Repositories;
using Application.UseCases.Categories.Dto.Request;
using Application.UseCases.Categories.Dto.Response;
using Application.UseCases.Categories.Interfaces;
using CoreLib.Audit;
using CoreLib.User;

namespace Application.UseCases.Categories;

internal class CategoryUseCaseManager(ICategoryRepository repository, IAuditService auditService, ICurrentUserService currentUser) : ICategoryUseCaseManager
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<GetCategoryResponse> GetCategoryAsync(Guid categoryId)
    {
        var category = await repository.GetCategoryByIdAsync(categoryId);

        return new GetCategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            IsSystem = category.IsSystem,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    /// <inheritdoc/>
    public async Task<AddCategoryResponse> AddCategoryAsync(AddCategoryRequest request)
    {
        var categoryId = await repository.AddCategoryAsync(request.Name, request.Description);
        await auditService.LogAsync("CreateCategory", "Category", categoryId, currentUser.UserId, currentUser.UserName, $"Создана категория «{request.Name}»");
        return new AddCategoryResponse
        {
            CategoryId = categoryId
        };
    }

    /// <inheritdoc/>
    public async Task<PatchCategoryResponse> UpdateCategoryAsync(Guid categoryId, PatchCategoryRequest request)
    {
        var category = await repository.UpdateCategoryAsync(categoryId, request.Name, request.Description, request.IsActive);
        await auditService.LogAsync("UpdateCategory", "Category", categoryId, currentUser.UserId, currentUser.UserName, $"Обновлена категория {categoryId}");
        return new PatchCategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            IsSystem = category.IsSystem,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    /// <inheritdoc/>
    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        await repository.DeleteCategoryAsync(categoryId);
        await auditService.LogAsync("DeleteCategory", "Category", categoryId, currentUser.UserId, currentUser.UserName, $"Удалена категория {categoryId}");
    }
}