using Application.UseCases.Images.Dto.Request;
using Application.UseCases.Images.Dto.Response;
using Application.UseCases.Images.Interfaces;
using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using CoreLib.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectManage;

/// <summary>
/// Контроллер управления информацией о проектах.
/// </summary>
[Route("api/project")]
[ApiController]
[Authorize]
public class ProjectController(IProjectUseCaseManager useCaseManager, IImageUseCase imageUseCase) : ControllerBase
{
    /// <summary>
    /// Получение списка объектов для главной страницы
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetSimpleProjectListResponse> GetSimpleProjectListAsync()
    {
        return await useCaseManager.GetSimpleProjectListAsync();
    }

    /// <summary>
    /// Получить полную информацию о проекте по идентификатору
    /// </summary>
    [HttpGet("{projectId}")]
    public async Task<GetFullProjectResponse> GetFullProjectByIdAsync([FromRoute] Guid projectId)
    {
        return await useCaseManager.GetFullProjectByIdAsync(projectId);
    }
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        request.Publisher ??= User.GetUserFio();
        return await useCaseManager.CreateProjectAsync(request);
    }
    
    /// <summary>
    /// Опубликовать проект
    /// </summary>
    [HttpPost("publish")]
    public async Task PublicateProjectAsync(PublicateProjectRequest request)
    {
        await useCaseManager.PublicateProjectAsync(request);
    }

    /// <summary>
    /// Обновить информацию о проекте
    /// </summary>
    [HttpPatch("update/{projectId}")]
    public async Task UpdateProjectAsync([FromRoute] Guid projectId, UpdateProjectRequest request)
    {
        await useCaseManager.UpdateProjectAsync(projectId, request);
    }

    /// <summary>
    /// Удалить проект
    /// </summary>
    [HttpDelete("delete/{projectId}")]
    public async Task DeleteProjectById([FromRoute] Guid projectId)
    {
        await useCaseManager.DeleteProjectAsync(projectId);
    }

    /// <summary>
    /// Получить черновик проекта
    /// </summary>
    [HttpGet("draft/{projectId}")]
    public async Task<GetProjectDraftResponse> GetProjectDraftAsync([FromRoute] Guid projectId)
    {
        return await useCaseManager.GetProjectDraftByIdAsync(projectId);
    }
    
    /// <summary>
    /// Загрузить картинку проекта
    /// </summary>
    [HttpPost("{projectId}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadProjectImageAsync([FromRoute] Guid projectId, [FromForm] UploadProjectImageRequest request)
    {
        await imageUseCase.AddProjectImageAsync(projectId, request);
        return Ok();
    }
    
    /// <summary>
    /// Получить список картинок проекта
    /// </summary>
    [HttpGet("{projectId}/images")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectImageResponse>>> GetProjectImagesAsync([FromRoute] Guid projectId)
    {
        var images = await imageUseCase.GetProjectImagesAsync(projectId);
        return Ok(images);
    }

    /// <summary>
    /// Удалить картинку проекта
    /// </summary>
    [HttpDelete("{projectId}/images/{imageId}")]
    public async Task<IActionResult> DeleteProjectImageAsync([FromRoute] Guid projectId, [FromRoute] Guid imageId)
    {
        await imageUseCase.DeleteProjectImageAsync(projectId, imageId);
        return Ok();
    }
}