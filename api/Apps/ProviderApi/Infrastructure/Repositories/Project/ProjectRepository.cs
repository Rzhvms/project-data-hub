using System.Data;
using Application.Ports.Repositories;
using Application.UseCases.ProjectManage.Dto.Request;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Dapper;
using Domain.Entities.Project;

namespace Infrastructure.Repositories.Project;

/// <inheritdoc/>
internal class ProjectRepository(IDbConnection dbConnection) : IProjectRepository
{
    /// <inheritdoc/>
    public async Task<List<ProjectCard>> GetSimpleProjectListAsync()
    {
        var sql = $@"SELECT 
                    {EntityMapper.ColName<ProjectCard>(x => x.Id)},
                    {EntityMapper.ColName<ProjectCard>(x => x.Title)},
                    {EntityMapper.ColName<ProjectCard>(x => x.CityRegion)},
                    {EntityMapper.ColName<ProjectCard>(x => x.ProjectStatus)},
                    {EntityMapper.ColName<ProjectCard>(x => x.CreatedAt)},
                    {EntityMapper.ColName<ProjectCard>(x => x.UpdatedAt)},
                    {EntityMapper.ColName<ProjectCard>(x => x.ObjectType)},
                    {EntityMapper.ColName<ProjectCard>(x => x.Publisher)},
                    {EntityMapper.ColName<ProjectCard>(x => x.PublicationStatus)}
                    FROM {EntityMapper.TbName<ProjectCard>()}";

        var projectList = await dbConnection.QueryAsync<ProjectCard>(sql);
        return projectList.AsList();
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateProjectAsync(ProjectCard projectCard)
    {
        var sql = $@"INSERT INTO {EntityMapper.TbName<ProjectCard>()} VALUES 
                                              (@Id, @Title, @ShortTitle, @CityRegion, @Address, @DesignYearPeriod, 
                                               @RealizationYear, @ProjectStatus, @ObjectType, @Customer, @InpadRole,
                                               @DesignStage, @ShortDescription, @LongDescription, @PublicationStatus,
                                               @CreatedAt, @UpdatedAt, @Publisher)";
        
        await dbConnection.ExecuteAsync(sql, projectCard);
        return projectCard.Id;
    }
}