using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project;

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
                       {EntityMapper.ColName<ProjectImage>(x => x.UseInPortfolio)}
                   )
                   VALUES
                   (
                       @Id, @ProjectId, @ObjectPath, @Title, @Description, @AlternativeText, @UseInSite, @UseInPresentation, @UseInPortfolio
                   )
                   """;

        await connection.ExecuteAsync(sql, new
        {
            image.Id, image.ProjectId, image.ObjectPath, image.Title, image.Description, image.AlternativeText,
            image.UseInSite, image.UseInPresentation, image.UseInPortfolio
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

//     /// <inheritdoc />
//     public async Task<ProjectImage?> GetMainImageAsync(Guid projectId)
//     {
//         var sql = $"""
//                    SELECT *
//                    FROM {EntityMapper.TbName<ProjectImage>()}
//                    WHERE {EntityMapper.ColName<ProjectImage>(x => x.ProjectId)} = @ProjectId
//                    AND {EntityMapper.ColName<ProjectImage>(x => x.IsMain)} = true
//                    LIMIT 1
//                    """;
//
//         var image = await connection.QueryFirstOrDefaultAsync<ProjectImage>(sql, new { ProjectId = projectId });
//
//         return image;
//     }

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
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.Id)} = @Id;
                   """;

        await connection.ExecuteAsync(sql, new
        {
            image.Id, image.ObjectPath, image.Title, image.Description, image.AlternativeText,
            image.UseInSite, image.UseInPresentation, image.UseInPortfolio
        });

        return image;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid imageId)
    {
        var sql = $"""
                   DELETE FROM {EntityMapper.TbName<ProjectImage>()}
                   WHERE {EntityMapper.ColName<ProjectImage>(x => x.Id)} = @Id;
                   """;

        await connection.ExecuteAsync(sql, new { Id = imageId });
    }
}