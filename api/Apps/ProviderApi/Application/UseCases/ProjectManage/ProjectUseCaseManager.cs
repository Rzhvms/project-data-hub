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
internal class ProjectUseCaseManager(IProjectRepository projectRepository, ICategoryRepository categoryRepository)
    : IProjectUseCaseManager
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
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                Publisher = project.Publisher,
                PublicationStatus = project.PublicationStatus.ToString()
            }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<GetFullProjectResponse> GetFullProjectByIdAsync(Guid projectId)
    {
        var project = await projectRepository.GetFullProjectByIdAsync(projectId);
        var categoryId = await categoryRepository.GetCategoryIdByProjectIdAsync(projectId);

        return new GetFullProjectResponse()
        {
            Id = project.Id,
            Title = project.Title,
            ShortTitle = project.ShortTitle,
            CityRegion = project.CityRegion,
            Address = project.Address,
            DesignYearPeriod = project.DesignYearPeriod,
            RealizationYear = project.RealizationYear,
            ProjectStatus = project.ProjectStatus,
            ObjectType = project.ObjectType,
            Customer = project.Customer,
            InpadRole = project.InpadRole,
            DesignStage = project.DesignStage,
            ShortDescription = project.ShortDescription,
            LongDescription = project.LongDescription,
            PublicationStatus = project.PublicationStatus.ToString(),
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Publisher = project.Publisher,
            CategoryId = categoryId
        };
    }

    /// <inheritdoc/>
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        var projectId = Guid.NewGuid();
        var project = new ProjectCard()
        {
            Id = projectId,
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

        var draftData = FromRequest(projectId, request);
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

    /// <inheritdoc/>
    public async Task UpdateProjectAsync(Guid projectId, UpdateProjectRequest request)
    {
        var draftData = FromRequest(projectId, request);

        var transaction = projectRepository.BeginTransaction();
        try
        {
            await projectRepository.UpdateProjectAsync(draftData, transaction);

            if (request.CategoryId != Guid.Empty)
            {
                var checkExist =
                    await categoryRepository.CheckExistProjectCategoryLink(projectId, request.CategoryId, transaction);
                if (!checkExist)
                {
                    await categoryRepository.AddProjectCategoryLink(projectId, request.CategoryId, transaction);
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteProjectAsync(Guid projectId)
    {
        await projectRepository.DeleteProjectAsync(projectId);
    }

    /// <summary>
    /// Маппинг в ProjectDraftData
    /// </summary>
    private static ProjectDraftData FromRequest<T>(Guid projectId, T request)
    {
        return request switch
        {
            CreateProjectRequest createProjectRequest => new ProjectDraftData
            {
                ProjectId = projectId,
                Title = createProjectRequest.Title,
                ShortTitle = createProjectRequest.ShortTitle,
                CityRegion = createProjectRequest.CityRegion,
                Address = createProjectRequest.Address,
                DesignYearPeriod = createProjectRequest.DesignYearPeriod,
                RealizationYear = createProjectRequest.RealizationYear,
                ProjectStatus = createProjectRequest.ProjectStatus,
                ObjectType = createProjectRequest.ObjectType,
                Customer = createProjectRequest.Customer,
                InpadRole = createProjectRequest.InpadRole,
                DesignStage = createProjectRequest.DesignStage,
                ShortDescription = createProjectRequest.ShortDescription,
                LongDescription = createProjectRequest.LongDescription,
                Publisher = createProjectRequest.Publisher,
                CategoryId = createProjectRequest.CategoryId,
            },
            UpdateProjectRequest updateProjectRequest => new ProjectDraftData
            {
                ProjectId = projectId,
                Title = updateProjectRequest.Title,
                ShortTitle = updateProjectRequest.ShortTitle,
                CityRegion = updateProjectRequest.CityRegion,
                Address = updateProjectRequest.Address,
                DesignYearPeriod = updateProjectRequest.DesignYearPeriod,
                RealizationYear = updateProjectRequest.RealizationYear,
                ProjectStatus = updateProjectRequest.ProjectStatus,
                ObjectType = updateProjectRequest.ObjectType,
                Customer = updateProjectRequest.Customer,
                InpadRole = updateProjectRequest.InpadRole,
                DesignStage = updateProjectRequest.DesignStage,
                ShortDescription = updateProjectRequest.ShortDescription,
                LongDescription = updateProjectRequest.LongDescription,
                Publisher = updateProjectRequest.Publisher,
                CategoryId = updateProjectRequest.CategoryId
            },
            _ => new ProjectDraftData()
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
        if (string.IsNullOrWhiteSpace(draft.CategoryId.ToString())) missing.Add(nameof(draft.CategoryId));

        if (missing.Count > 0)
        {
            throw new InvalidProjectCardException(
                $"Нельзя опубликовать проект. Не заполнены поля: {string.Join(", ", missing)}");
        }
    }
}