using Application.UseCases.Categories.Dto.Response;
using Application.UseCases.Categories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Categories;

[Authorize]
[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryUseCaseManager useCaseManager) : ControllerBase
{
    /// <summary>
    /// Получить список доступных категорий
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(GetCategoryListResponse), 200)]
    [ProducesResponseType(typeof(GetCategoryListResponse), 400)]
    public async Task<GetCategoryListResponse> GetCategoriesAsync()
    {
        return await useCaseManager.GetCategoriesAsync();
    }
}