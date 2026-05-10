using Application.Ports.Repositories;
using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;
using Domain.Entities.Project;

namespace Application.UseCases.ProjectManage;

/// <inheritdoc/>
internal class ProjectUseCaseManager(IProjectRepository projectRepository) : IProjectUseCaseManager
{
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
            PublicationStatus = ProjectPublicationStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            Publisher = request.Publisher
        };
        
        var response = await projectRepository.CreateProjectAsync(project);
        
        return new CreateProjectResponse
        {
            ProjectId = response
        };
    }
}