using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project;
using Serilog;

namespace Infrastructure.Repositories.Project;

/// <inheritdoc />
public class ProjectImageRepository(IDbConnection connection) : IImageRepository
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(ProjectImage image)
    {
        var sql = $"""
                   INSERT INTO {EntityMapper.TbName<ProjectImage>()}
                   (
                       {EntityMapper.ColName<ProjectImage>(x => x.Id)},
                       {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)},
                       {EntityMapper.ColName<ProjectImage>(x => x.ObjectPath)},
                       {EntityMapper.ColName<ProjectImage>(x => x.Title)},
                       {EntityMapper.ColName<ProjectImage>(x => x.Description)},
                       {EntityMapper.ColName<ProjectImage>(x => x.AlternativeText)},
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInSite)},
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInPresentation)},
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInPortfolio)},
                       {EntityMapper.ColName<ProjectImage>(x => x.IsMain)}
                   )
                   VALUES
                   (
                       @Id, @ProjectId, @ObjectPath, @Title, @Description, @AlternativeText, @UseInSite, @UseInPresentation, @UseInPortfolio, @IsMain
                   )
                   """;

        if (image.IsMain)
        {
            await SetIsMainFalseAsync(image.ProjectId);
        }
        
        await connection.ExecuteAsync(sql, new
        {
            image.Id, image.ProjectId, image.ObjectPath, image.Title, image.Description, image.AlternativeText,
            image.UseInSite, image.UseInPresentation, image.UseInPortfolio, image.IsMain
        });

        return image.Id;
    }

    /// <inheritdoc />
    public async Task<ProjectImage> GetByIdAsync(Guid imageId)
    {
        var sql = $"""
                   SELECT *
                   FROM {EntityMapper.TbName<ProjectImage>()}
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.Id)} = @Id
                   """;

        var image = await connection.QueryFirstOrDefaultAsync<ProjectImage>(sql, new { Id = imageId });
        
        return image ?? throw new EntityNotFoundException("Изображение не найдено");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProjectImage>> GetAllByProjectIdAsync(Guid projectId)
    {
        var sql = $"""
                   SELECT *
                   FROM {EntityMapper.TbName<ProjectImage>()}
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)} = @ProjectId
                   """;

        return await connection.QueryAsync<ProjectImage>(sql, new { ProjectId = projectId });
    }

     /// <inheritdoc />
     public async Task<ProjectImage?> GetMainImageAsync(Guid projectId)
     {
         var sql = $"""
                    SELECT *
                    FROM {EntityMapper.TbName<ProjectImage>()}
                    WHERE {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)} = @ProjectId
                    AND {EntityMapper.ColName<ProjectImage>(x => x.IsMain)} = true
                    LIMIT 1
                    """;

         var image = await connection.QueryFirstOrDefaultAsync<ProjectImage>(sql, new { ProjectId = projectId });

         return image;
     }

    /// <inheritdoc />
    public async Task<ProjectImage> UpdateAsync(ProjectImage image)
    {
        var sql = $"""
                   UPDATE {EntityMapper.TbName<ProjectImage>()}
                   SET {EntityMapper.ColName<ProjectImage>(x => x.ObjectPath)} = @ObjectPath,
                       {EntityMapper.ColName<ProjectImage>(x => x.Title)} = @Title,
                       {EntityMapper.ColName<ProjectImage>(x => x.Description)} = @Description,
                       {EntityMapper.ColName<ProjectImage>(x => x.AlternativeText)} = @AlternativeText,
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInSite)} = @UseInSite,
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInPortfolio)} = @UseInPortfolio,
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInPresentation)} = @UseInPresentation,
                       {EntityMapper.ColName<ProjectImage>(x => x.IsMain)} = @IsMain
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.Id)} = @Id;
                   """;

        if (image.IsMain)
        {
            await SetIsMainFalseAsync(image.ProjectId);
        }
        
        await connection.ExecuteAsync(sql, new
        {
            image.Id, image.ObjectPath, image.Title, image.Description, image.AlternativeText,
            image.UseInSite, image.UseInPresentation, image.UseInPortfolio, image.IsMain
        });

        return image;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid projectId, Guid imageId)
    {
        var sql = $"""
                   DELETE FROM {EntityMapper.TbName<ProjectImage>()}
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.Id)} = @Id
                   AND {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)} = @ProjectId;
                   """;

        await connection.ExecuteAsync(sql, new { Id = imageId, ProjectId = projectId });
    }
    
    /// <summary>
    /// Установить IsMain = false. Гарант, что isMain всегда будет один
    /// </summary>
    private async Task SetIsMainFalseAsync(Guid projectId)
    {
        var sql = $"""
                    UPDATE {EntityMapper.TbName<ProjectImage>()}
                    SET {EntityMapper.ColName<ProjectImage>(x => x.IsMain)} = false
                    WHERE {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)} = @ProjectId;
                   """;
        try
        {
            await connection.ExecuteAsync(sql, new { ProjectId = projectId });
        }
        catch
        {
            Log.Error("Произошла ошибка при установке IsMain = false для проекта {ProjectId}", projectId);
        }
    }
}