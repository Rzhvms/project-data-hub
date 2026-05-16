using Application.UseCases.Categories.Dto.Response;

namespace Application.UseCases.Categories.Interfaces;

public interface ICategoryUseCaseManager
{
    Task<GetCategoryListResponse> GetCategoriesAsync();
}