using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project.Categories;
using Npgsql;

namespace Infrastructure.Repositories.Categories;

internal class CategoryRepository(IDbConnection dbConnection) : ICategoryRepository
{
    public async Task<List<ProjectCategory>> GetAllCategories()
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<ProjectCategory>()}";
        var categories = await dbConnection.QueryAsync<ProjectCategory>(sql);
        return categories.AsList();
    }

    public async Task AddProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null)
    {
        try
        {
            var projectCategoryLinkSql =
                $@"INSERT INTO {EntityMapper.TbName<ProjectCategoryLink>()} VALUES (@Id, @ProjectId, @CategoryId)";

            await dbConnection.ExecuteAsync(projectCategoryLinkSql, new
            {
                Id = Guid.NewGuid(), ProjectId = projectId, CategoryId = categoryId
            }, transaction);
        }
        catch (PostgresException)
        {
            throw new EntityNotFoundException("Передан некорректный идентификатор категории");
        }
    }

    public async Task<bool> CheckExistProjectCategoryLink(Guid projectId, Guid categoryId, IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<ProjectCategoryLink>()}
                    WHERE {EntityMapper.ColName<ProjectCategoryLink>(x => x.ProjectId)} = @ProjectId
                    AND {EntityMapper.ColName<ProjectCategoryLink>(x => x.CategoryId)} = @CategoryId";
        
        var res = await dbConnection.QueryAsync<ProjectCategoryLink>(sql, new
        {
            ProjectId = projectId, CategoryId = categoryId
        }, transaction);
        
        return res.Any();
    }

    public async Task<Guid> GetCategoryIdByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT {EntityMapper.ColName<ProjectCategoryLink>(x => x.CategoryId)} 
                    FROM {EntityMapper.TbName<ProjectCategoryLink>()} 
                    WHERE {EntityMapper.ColName<ProjectCategoryLink>(x => x.ProjectId)} = @ProjectId";
        
        return await dbConnection.QuerySingleOrDefaultAsync<Guid>(sql, new { ProjectId = projectId }, transaction);
    }
}