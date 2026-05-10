using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project;

namespace Infrastructure.Repositories.Project;

/// <inheritdoc/>
internal class ProjectRepository(IDbConnection dbConnection) : IProjectRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
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
    public async Task<Guid> CreateProjectAsync(ProjectCard projectCard, ProjectDraftData draftData)
    {
        var transaction = dbConnection.BeginTransaction();

        try
        {
            var insertProjectCard = $@"INSERT INTO {EntityMapper.TbName<ProjectCard>()} VALUES 
                                              (@Id, @Title, @ShortTitle, @CityRegion, @Address, @DesignYearPeriod, 
                                               @RealizationYear, @ProjectStatus, @ObjectType, @Customer, @InpadRole,
                                               @DesignStage, @ShortDescription, @LongDescription, @PublicationStatus,
                                               @CreatedAt, @UpdatedAt, @Publisher)";

            await dbConnection.ExecuteAsync(insertProjectCard, projectCard, transaction);

            var projectData = JsonSerializer.Serialize(draftData, JsonOptions);

            var insertToDraft =
                $@"INSERT INTO {EntityMapper.TbName<ProjectCardDraft>()} VALUES (@Id, @ProjectId, @ProjectData::jsonb)";

            await dbConnection.ExecuteAsync(insertToDraft, new 
                { Id = Guid.NewGuid(), ProjectId = projectCard.Id, ProjectData = projectData }, transaction);
            
            transaction.Commit();
            return projectCard.Id;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    /// <inheritdoc/>
    public async Task<ProjectCardDraft?> GetDraftByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT
                    {EntityMapper.ColName<ProjectCardDraft>(x => x.Id)},
                    {EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectId)},
                    {EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectData)}
                    FROM {EntityMapper.TbName<ProjectCardDraft>()}
                    WHERE {EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectId)} = @ProjectId;";

        return await dbConnection.QuerySingleOrDefaultAsync<ProjectCardDraft>(sql, new { ProjectId = projectId }, transaction);
    }
    
    /// <inheritdoc/>
    public async Task PublishProjectAsync(Guid projectId, ProjectDraftData draftData, IDbTransaction? transaction = null)
    {
        var sql = $@"UPDATE {EntityMapper.TbName<ProjectCard>()}
                    SET
                        {EntityMapper.ColName<ProjectCard>(x => x.Title)} = @Title,
                        {EntityMapper.ColName<ProjectCard>(x => x.ShortTitle)} = @ShortTitle,
                        {EntityMapper.ColName<ProjectCard>(x => x.CityRegion)} = @CityRegion,
                        {EntityMapper.ColName<ProjectCard>(x => x.Address)} = @Address,
                        {EntityMapper.ColName<ProjectCard>(x => x.DesignYearPeriod)} = @DesignYearPeriod,
                        {EntityMapper.ColName<ProjectCard>(x => x.RealizationYear)} = @RealizationYear,
                        {EntityMapper.ColName<ProjectCard>(x => x.ProjectStatus)} = @ProjectStatus,
                        {EntityMapper.ColName<ProjectCard>(x => x.ObjectType)} = @ObjectType,
                        {EntityMapper.ColName<ProjectCard>(x => x.Customer)} = @Customer,
                        {EntityMapper.ColName<ProjectCard>(x => x.InpadRole)} = @InpadRole,
                        {EntityMapper.ColName<ProjectCard>(x => x.DesignStage)} = @DesignStage,
                        {EntityMapper.ColName<ProjectCard>(x => x.ShortDescription)} = @ShortDescription,
                        {EntityMapper.ColName<ProjectCard>(x => x.LongDescription)} = @LongDescription,
                        {EntityMapper.ColName<ProjectCard>(x => x.PublicationStatus)} = @PublicationStatus,
                        {EntityMapper.ColName<ProjectCard>(x => x.UpdatedAt)} = @UpdatedAt,
                        {EntityMapper.ColName<ProjectCard>(x => x.Publisher)} = @Publisher
                    WHERE {EntityMapper.ColName<ProjectCard>(x => x.Id)} = @ProjectId;";

        await dbConnection.ExecuteAsync(sql, new
        {
            ProjectId = projectId,
            draftData.Title,
            draftData.ShortTitle,
            draftData.CityRegion,
            draftData.Address,
            draftData.DesignYearPeriod,
            draftData.RealizationYear,
            draftData.ProjectStatus,
            draftData.ObjectType,
            draftData.Customer,
            draftData.InpadRole,
            draftData.DesignStage,
            draftData.ShortDescription,
            draftData.LongDescription,
            draftData.Publisher,
            PublicationStatus = ProjectPublicationStatus.Published,
            UpdatedAt = DateTime.UtcNow
        }, transaction);
    }

    /// <inheritdoc/>
    public async Task UpdatePublicationStatusAsync(Guid projectId, ProjectPublicationStatus status, IDbTransaction? transaction = null)
    {
        var sql = $@"UPDATE {EntityMapper.TbName<ProjectCard>()} 
                    SET {EntityMapper.ColName<ProjectCard>(x => x.PublicationStatus)} = @PublicationStatus
                    WHERE {EntityMapper.ColName<ProjectCard>(x => x.Id)} = @ProjectId";
        
        await dbConnection.ExecuteAsync(sql, new { PublicationStatus = status, ProjectId = projectId }, transaction);
    }

    /// <inheritdoc/>
    public async Task RemoveDraftByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null)
    {
        var sql = $@"DELETE FROM {EntityMapper.TbName<ProjectCardDraft>()} 
                     WHERE {EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectId)} = @ProjectId";
        
        var deleted = await dbConnection.ExecuteAsync(sql, new { ProjectId = projectId }, transaction);
        if (deleted == 0)
        {
            throw new EntityNotFoundException("Черновик не найден");
        }
    }

    /// <inheritdoc/>
    public IDbTransaction BeginTransaction()
    {
        return dbConnection.BeginTransaction();
    }
}