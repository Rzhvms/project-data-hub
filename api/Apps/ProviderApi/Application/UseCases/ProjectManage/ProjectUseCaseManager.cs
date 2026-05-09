using Application.Ports.Repositories;
using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;
using Application.UseCases.ProjectManage.Interfaces;

namespace Application.UseCases.ProjectManage;

/// <inheritdoc/>
internal class ProjectUseCaseManager(IProjectRepository projectRepository) : IProjectUseCaseManager
{
    /// <inheritdoc/>
    public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
    {
        throw new NotImplementedException();
    }
}