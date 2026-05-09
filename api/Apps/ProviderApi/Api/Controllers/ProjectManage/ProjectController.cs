using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectManage;

/// <summary>
/// Контроллер управления информацией о проектах.
/// </summary>
[Route("api/project")]
[ApiController]
public class ProjectController(IProjectUseCaseManager useCaseManager) : ControllerBase
{
    // public async Task GetProjectListAsync()
    // {
    //     
    // }
    
    /// <summary>
    /// Создание проекта
    /// </summary>
    [HttpPost("create")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        return await useCaseManager.CreateProjectAsync(request);
    }
    
    
}