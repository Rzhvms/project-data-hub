using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project;

namespace Infrastructure.Repositories.Project;

/// <inheritdoc/>
internal sealed class ProjectMetricsRepository(IDbConnection dbConnection) : IProjectMetricsRepository
{
    /// <inheritdoc/>
    public async Task<ProjectMetrics> GetMetricsByProjectIdAsync(Guid projectId)
    {
        var query = $@"SELECT * FROM {EntityMapper.TbName<ProjectMetrics>()}
                       WHERE {EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)} = @ProjectId";

        var project = await dbConnection.QuerySingleOrDefaultAsync<ProjectMetrics>(query, new { ProjectId = projectId });
        if (project is null)
        {
            throw new EntityNotFoundException($"Технико-экономические показатели не найдены для проекта {projectId}");
        }
        return project;
    }

    /// <inheritdoc/>
    public async Task<Guid> AddProjectMetricsAsync(ProjectMetrics metrics, IDbTransaction? transaction = null)
    {
        var queryCheck = $@"SELECT {EntityMapper.ColName<ProjectMetrics>(x => x.Id)}
                            FROM {EntityMapper.TbName<ProjectMetrics>()} 
                            WHERE {EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)} = @ProjectId";
        
        var rowsAffected = await dbConnection.QueryAsync<Guid>(queryCheck, new { metrics.ProjectId }, transaction);
        if (rowsAffected.Any())
        {
            var deleteQuery = $@"DELETE FROM {EntityMapper.TbName<ProjectMetrics>()} 
                                 WHERE {EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)} = @ProjectId";
            await dbConnection.ExecuteAsync(deleteQuery, metrics, transaction);
        }
        
        var query = $@"INSERT INTO {EntityMapper.TbName<ProjectMetrics>()} VALUES 
                       (@Id, @ProjectId, @TotalArea, @SiteArea, @BuildingArea, @BuildingsCount, @Floors, @ApartmentsCount, 
                        @ParkingSpacesCount, @JsonData::jsonb)";
        
        await dbConnection.ExecuteAsync(query, metrics, transaction);
        return metrics.ProjectId;
    }

    /// <inheritdoc/>
    public async Task DeleteMetricsByProjectIdAsync(Guid projectId)
    {
        var query = $@"DELETE FROM {EntityMapper.TbName<ProjectMetrics>()} 
                       WHERE {EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)} = @ProjectId";
        
        var deleted = await dbConnection.ExecuteAsync(query, new { ProjectId = projectId });
        if (deleted == 0)
        {
            throw new EntityNotFoundException($"Технико-экономические показатели не найдены для проекта {projectId}");
        }
    }

    /// <inheritdoc/>
    public async Task<ProjectMetrics> PutProjectMetricsByProjectId(ProjectMetrics metrics, IDbTransaction? transaction = null)
    {
        var query = $@"UPDATE {EntityMapper.TbName<ProjectMetrics>()}
                       SET {EntityMapper.ColName<ProjectMetrics>(x => x.TotalArea)} = @TotalArea,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.SiteArea)} = @SiteArea,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.BuildingArea)} = @BuildingArea,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.BuildingsCount)} = @BuildingsCount,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.Floors)} = @Floors,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.ApartmentsCount)} = @ApartmentsCount,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.ParkingSpacesCount)} = @ParkingSpacesCount,
                           {EntityMapper.ColName<ProjectMetrics>(x => x.JsonData)} = @JsonData::jsonb
                       WHERE {EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)} = @ProjectId";
        
        await dbConnection.ExecuteAsync(query, new
        {
            metrics.TotalArea, metrics.SiteArea, metrics.BuildingArea, metrics.BuildingsCount, metrics.Floors, 
            metrics.ApartmentsCount, metrics.ParkingSpacesCount, metrics.JsonData, metrics.ProjectId
        }, transaction);
        return metrics;
    }
}