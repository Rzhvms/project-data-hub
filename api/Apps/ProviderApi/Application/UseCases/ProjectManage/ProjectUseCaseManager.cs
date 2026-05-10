using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Ports.Repositories;
using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using CoreLib.Exceptions;
using Domain.Entities.Project;

namespace Application.UseCases.ProjectManage;

/// <inheritdoc/>
internal class ProjectUseCaseManager(IProjectRepository projectRepository) : IProjectUseCaseManager
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    /// <inheritdoc/>
    public async Task<GetSimpleProjectListResponse> GetSimpleProjectListAsync()
    {
        var projects = await projectRepository.GetSimpleProjectListAsync();
        return new GetSimpleProjectListResponse
        {
            ProjectList = projects.Select(project => new GetSimpleProjectResponse()
            {
                Id = project.Id,
                Title = project.Title,
                Place = project.CityRegion,
                ObjectType = project.ObjectType,
                ProjectStatus = project.ProjectStatus,
                CreatedAt  = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                Publisher = project.Publisher,
                PublicationStatus = project.PublicationStatus.ToString()
            }).ToList()
        };
    }
    
    /// <inheritdoc/>
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        var project = new ProjectCard()
        {
            Id = Guid.NewGuid(),
            Title = request.Title ?? string.Empty,
            ShortTitle = request.ShortTitle,
            CityRegion = request.CityRegion ?? string.Empty,
            Address = request.Address,
            DesignYearPeriod = request.DesignYearPeriod,
            RealizationYear = request.RealizationYear,
            ProjectStatus = request.ProjectStatus ?? string.Empty,
            ObjectType = request.ObjectType ?? string.Empty,
            Customer = request.Customer,
            InpadRole = request.InpadRole ?? string.Empty,
            DesignStage = request.DesignStage,
            ShortDescription = request.ShortDescription ?? string.Empty,
            LongDescription = request.LongDescription,
            PublicationStatus = ProjectPublicationStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            Publisher = request.Publisher
        };
        
        var draftData = FromCreateProjectRequest(request);
        var response = await projectRepository.CreateProjectAsync(project, draftData);
        
        return new CreateProjectResponse
        {
            ProjectId = response
        };
    }

    /// <inheritdoc/>
    public async Task PublicateProjectAsync(PublicateProjectRequest request)
    {
        using var transaction = projectRepository.BeginTransaction();

        try
        {
            var draft = await projectRepository.GetDraftByProjectIdAsync(request.ProjectId, transaction)
                        ?? throw new EntityNotFoundException("Черновик не найден");

            var draftData = JsonSerializer.Deserialize<ProjectDraftData>(draft.ProjectData, JsonOptions)
                            ?? throw new InvalidProjectCardException("Черновик пустой");

            ValidateDraft(draftData);

            await projectRepository.PublishProjectAsync(request.ProjectId, draftData, transaction);
            await projectRepository.RemoveDraftByProjectIdAsync(request.ProjectId, transaction);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    /// <summary>
    /// Маппинг в ProjectDraftData
    /// </summary>
    private static ProjectDraftData FromCreateProjectRequest(CreateProjectRequest request)
    {
        return new ProjectDraftData
        {
            Title = request.Title,
            ShortTitle = request.ShortTitle,
            CityRegion = request.CityRegion,
            Address = request.Address,
            DesignYearPeriod = request.DesignYearPeriod,
            RealizationYear = request.RealizationYear,
            ProjectStatus = request.ProjectStatus,
            ObjectType = request.ObjectType,
            Customer = request.Customer,
            InpadRole = request.InpadRole,
            DesignStage = request.DesignStage,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            Publisher = request.Publisher
        };
    }
    
    /// <summary>
    /// Валидация черновика
    /// </summary>
    private static void ValidateDraft(ProjectDraftData draft)
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(draft.Title)) missing.Add(nameof(draft.Title));
        if (string.IsNullOrWhiteSpace(draft.CityRegion)) missing.Add(nameof(draft.CityRegion));
        if (string.IsNullOrWhiteSpace(draft.ProjectStatus)) missing.Add(nameof(draft.ProjectStatus));
        if (string.IsNullOrWhiteSpace(draft.ObjectType)) missing.Add(nameof(draft.ObjectType));
        if (string.IsNullOrWhiteSpace(draft.InpadRole)) missing.Add(nameof(draft.InpadRole));
        if (string.IsNullOrWhiteSpace(draft.ShortDescription)) missing.Add(nameof(draft.ShortDescription));
        // TODO: добавить категории объекта

        if (missing.Count > 0)
        {
            throw new InvalidProjectCardException($"Нельзя опубликовать проект. Не заполнены поля: {string.Join(", ", missing)}");
        }
    }
}