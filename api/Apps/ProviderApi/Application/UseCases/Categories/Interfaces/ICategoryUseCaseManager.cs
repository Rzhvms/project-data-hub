using Application.UseCases.Categories.Dto.Request;
using Application.UseCases.Categories.Dto.Response;

namespace Application.UseCases.Categories.Interfaces;

public interface ICategoryUseCaseManager
{
    /// <summary>
    /// Получить список категорий
    /// </summary>
    Task<GetCategoryListResponse> GetCategoriesAsync();

    /// <summary>
    /// Получить категорию по идентификатору
    /// </summary>
    Task<GetCategoryResponse> GetCategoryAsync(Guid categoryId);

    /// <summary>
    /// Удалить категорию по идентификатору
    /// </summary>
    Task DeleteCategoryAsync(Guid categoryId);

    /// <summary>
    /// Добавить новую категорию
    /// </summary>
    Task<AddCategoryResponse> AddCategoryAsync(AddCategoryRequest request);

    /// <summary>
    /// Обновить категорию
    /// </summary>
    Task<PatchCategoryResponse> UpdateCategoryAsync(Guid categoryId, PatchCategoryRequest request);
}