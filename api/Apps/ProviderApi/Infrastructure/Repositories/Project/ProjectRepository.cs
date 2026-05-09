using System.Data;
using Application.Ports.Repositories;

namespace Infrastructure.Repositories.Project;

internal class ProjectRepository(IDbConnection dbConnection) : IProjectRepository
{
    
}