using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectManage;

[Authorize]
[ApiController]
[Route("api/project/metrics")]
public class ProjectMetricsController(IProjectMetricsUseCaseManager useCaseManager) : ControllerBase
{
    /// <summary>
    /// Получить метрики проекта
    /// </summary>
    [HttpGet("{projectId}")]
    [Authorize(Roles = "Viewer,Editor,Administrator")]
    [ProducesResponseType(typeof(GetProjectMetricsResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetProjectMetricsResponse),StatusCodes.Status400BadRequest)]
    public async Task<GetProjectMetricsResponse> GetMetricsByProjectIdAsync([FromRoute] Guid projectId)
    {
        return await useCaseManager.GetMetricsByProjectIdAsync(projectId);
    }

    /// <summary>
    /// Добавить метрики для проекта
    /// </summary>
    [HttpPost("{projectId}")]
    [Authorize(Roles = "Editor,Administrator")]
    [ProducesResponseType(typeof(AddProjectMetricsResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AddProjectMetricsResponse),StatusCodes.Status400BadRequest)]
    public async Task<AddProjectMetricsResponse> AddProjectMetricsAsync([FromRoute] Guid projectId, AddProjectMetricsRequest metrics)
    {
        return await useCaseManager.AddProjectMetricsAsync(projectId, metrics);
    }

    /// <summary>
    /// Удалить метрики для проекта
    /// </summary>
    [HttpDelete("{projectId}")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteMetricsByProjectIdAsync([FromRoute] Guid projectId)
    {
        await useCaseManager.DeleteMetricsByProjectIdAsync(projectId);
    }

    /// <summary>
    /// Обновить метрики проекта
    /// </summary>
    [HttpPut("{projectId}")]
    [Authorize(Roles = "Editor,Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<PutProjectMetricsResponse> PutProjectMetricsByProjectId([FromRoute] Guid projectId, PutProjectMetricsRequest request)
    {
        return await useCaseManager.PutProjectMetricsByProjectId(projectId, request);
    }
}