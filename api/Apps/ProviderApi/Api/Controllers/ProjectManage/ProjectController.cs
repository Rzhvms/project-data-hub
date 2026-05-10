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
public class ProjectController(IProjectUseCaseManager useCaseManager) : ControllerBase
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
    /// Создание проекта
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        request.Publisher ??= User.GetUserFio();
        return await useCaseManager.CreateProjectAsync(request);
    }

    [HttpPost("publish")]
    public async Task PublicateProjectAsync(PublicateProjectRequest request)
    {
        await useCaseManager.PublicateProjectAsync(request);
    }
}