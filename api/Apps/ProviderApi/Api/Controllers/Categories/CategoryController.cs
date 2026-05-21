using Application.UseCases.Categories.Dto.Request;
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
    [HttpGet("list")]
    [ProducesResponseType(typeof(GetCategoryListResponse), 200)]
    [ProducesResponseType(typeof(GetCategoryListResponse), 400)]
    public async Task<GetCategoryListResponse> GetCategoriesAsync()
    {
        return await useCaseManager.GetCategoriesAsync();
    }

    /// <summary>
    /// Получить категорию по идентификатору
    /// </summary>
    [HttpGet("{categoryId}")]
    [ProducesResponseType(typeof(GetCategoryResponse), 200)]
    [ProducesResponseType(typeof(GetCategoryResponse), 400)]
    public async Task<GetCategoryResponse> GetCategoryAsync([FromRoute] Guid categoryId)
    {
        return await useCaseManager.GetCategoryAsync(categoryId);
    }
    
    /// <summary>
    /// Добавить новую категорию
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(typeof(AddCategoryResponse), 200)]
    [ProducesResponseType(typeof(AddCategoryResponse), 400)]
    public async Task<AddCategoryResponse> AddCategoryAsync(AddCategoryRequest request)
    {
        return await useCaseManager.AddCategoryAsync(request);
    }

    /// <summary>
    /// Обновить данные категорий
    /// </summary>
    [HttpPatch("{categoryId}")]
    [ProducesResponseType(typeof(PatchCategoryResponse), 200)]
    [ProducesResponseType(typeof(PatchCategoryResponse), 400)]
    public async Task<PatchCategoryResponse> UpdateCategoryAsync([FromRoute] Guid categoryId, PatchCategoryRequest request)
    {
        return await useCaseManager.UpdateCategoryAsync(categoryId, request);
    }
    
    /// <summary>
    /// Удалить категорию по идентификатору
    /// </summary>
    [HttpDelete("delete/{categoryId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task DeleteCategoryAsync([FromRoute] Guid categoryId)
    {
        await useCaseManager.DeleteCategoryAsync(categoryId);
    }
}