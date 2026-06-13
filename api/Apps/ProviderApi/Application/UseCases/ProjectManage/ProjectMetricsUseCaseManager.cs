using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Application.Ports.Repositories;
using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using CoreLib.Audit;
using CoreLib.Exceptions;
using CoreLib.User;
using Domain.Entities.Project;

namespace Application.UseCases.ProjectManage;

/// <inheritdoc/>
public class ProjectMetricsUseCaseManager(
    IProjectMetricsRepository repository,
    IProjectRepository projectRepository,
    IAuditService auditService,
    ICurrentUserService currentUser) : IProjectMetricsUseCaseManager
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    /// <inheritdoc/>
    public async Task<GetProjectMetricsResponse> GetMetricsByProjectIdAsync(Guid projectId)
    {
        var metrics = await repository.GetMetricsByProjectIdAsync(projectId);
        return new GetProjectMetricsResponse
        {
            ProjectId = metrics.ProjectId,
            TotalArea = metrics.TotalArea,
            SiteArea = metrics.SiteArea,
            BuildingArea = metrics.BuildingArea,
            BuildingsCount = metrics.BuildingsCount,
            Floors = metrics.Floors,
            ApartmentsCount = metrics.ApartmentsCount,
            ParkingSpacesCount = metrics.ParkingSpacesCount,
            JsonData = metrics.JsonData
        };
    }

    /// <inheritdoc/>
    public async Task<AddProjectMetricsResponse> AddProjectMetricsAsync(Guid projectId, AddProjectMetricsRequest request)
    {
        var transaction = projectRepository.BeginTransaction();
        try
        {
            var metricsId = Guid.NewGuid();
            var metrics = new ProjectMetrics
            {
                Id = metricsId,
                ProjectId = projectId,
                TotalArea = request.TotalArea,
                SiteArea = request.SiteArea,
                BuildingArea = request.BuildingArea,
                BuildingsCount = request.BuildingsCount,
                Floors = request.Floors,
                ApartmentsCount = request.ApartmentsCount,
                ParkingSpacesCount = request.ParkingSpacesCount,
                JsonData = request.JsonData
            };

            // Получаем черновик
            var draft = await projectRepository.GetDraftByProjectIdAsync(projectId, transaction)
                        ?? throw new EntityNotFoundException("Черновик не найден");

            var draftData = draft.ProjectData.Deserialize<ProjectDraftData>(JsonOptions)
                            ?? throw new InvalidProjectCardException("Черновик пустой");
            
            draftData.ProjectMetrics = JsonSerializer.SerializeToNode(metrics, JsonOptions) as JsonObject;

            // Обновляем черновик
            await projectRepository.UpdateProjectDraftAsync(projectId, draftData, transaction);
        
            var response = await repository.AddProjectMetricsAsync(metrics, transaction);
            
            transaction.Commit();
            await auditService.LogAsync("AddProjectMetrics", "ProjectMetrics", projectId, currentUser.UserId, currentUser.UserName, $"Добавлены ТЭП проекта {projectId}");

            return new AddProjectMetricsResponse()
            {
                ProjectId = response
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteMetricsByProjectIdAsync(Guid projectId)
    {
        await repository.DeleteMetricsByProjectIdAsync(projectId);
        await auditService.LogAsync("DeleteProjectMetrics", "ProjectMetrics", projectId, currentUser.UserId, currentUser.UserName, $"Удалены ТЭП проекта {projectId}");
    }

    /// <inheritdoc/>
    public async Task<PutProjectMetricsResponse> PutProjectMetricsByProjectId(Guid projectId, PutProjectMetricsRequest request)
    {
        var transaction = projectRepository.BeginTransaction();
        try
        {
            var metrics = new ProjectMetrics
            {
                ProjectId = projectId,
                TotalArea = request.TotalArea,
                SiteArea = request.SiteArea,
                BuildingArea = request.BuildingArea,
                BuildingsCount = request.BuildingsCount,
                Floors = request.Floors,
                ApartmentsCount = request.ApartmentsCount,
                ParkingSpacesCount = request.ParkingSpacesCount,
                JsonData = request.JsonData
            };
            var response = await repository.PutProjectMetricsByProjectId(metrics, transaction);
        
            // Получаем черновик
            var draft = await projectRepository.GetDraftByProjectIdAsync(projectId)
                        ?? throw new EntityNotFoundException("Черновик не найден");

            var draftData = draft.ProjectData.Deserialize<ProjectDraftData>(JsonOptions)
                            ?? throw new InvalidProjectCardException("Черновик пустой");
        
            draftData.ProjectMetrics = JsonSerializer.SerializeToNode(metrics, JsonOptions) as JsonObject;
            
            // Обновляем черновик
            await projectRepository.UpdateProjectDraftAsync(projectId, draftData, transaction);
        
            transaction.Commit();
            await auditService.LogAsync("UpdateProjectMetrics", "ProjectMetrics", projectId, currentUser.UserId, currentUser.UserName, $"Обновлены ТЭП проекта {projectId}");

            return new PutProjectMetricsResponse
            {
                ProjectId = response.ProjectId,
                TotalArea = response.TotalArea,
                SiteArea = response.SiteArea,
                BuildingArea = response.BuildingArea,
                BuildingsCount = response.BuildingsCount,
                Floors = response.Floors,
                ApartmentsCount = response.ApartmentsCount,
                ParkingSpacesCount = response.ParkingSpacesCount,
                JsonData = response.JsonData
            };
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}