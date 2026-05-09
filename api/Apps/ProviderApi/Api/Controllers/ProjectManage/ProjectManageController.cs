using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectManage;

/// <summary>
/// Контроллер управления информацией о проектах.
/// </summary>
[Route("api/project")]
[ApiController]
public class ProjectManageController(IProjectUseCaseManager useCaseManager) : ControllerBase
{
    /// <summary>
    /// Создание проекта
    /// </summary>
    [HttpPost]
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        return await useCaseManager.CreateProjectAsync(request);
    }
}