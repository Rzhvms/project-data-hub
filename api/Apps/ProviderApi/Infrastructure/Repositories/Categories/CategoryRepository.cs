using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project.Categories;
using Npgsql;

namespace Infrastructure.Repositories.Categories;

/// <inheritdoc />
internal class CategoryRepository(IDbConnection dbConnection) : ICategoryRepository
{
    /// <inheritdoc />
    public async Task<List<ProjectCategory>> GetAllCategories()
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<ProjectCategory>()}";
        var categories = await dbConnection.QueryAsync<ProjectCategory>(sql);
        return categories.AsList();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<bool> CheckExistProjectCategoryLink(Guid projectId, Guid categoryId,
        IDbTransaction? transaction = null)
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

    /// <inheritdoc />
    public async Task<Guid> GetCategoryIdByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT {EntityMapper.ColName<ProjectCategoryLink>(x => x.CategoryId)} 
                    FROM {EntityMapper.TbName<ProjectCategoryLink>()} 
                    WHERE {EntityMapper.ColName<ProjectCategoryLink>(x => x.ProjectId)} = @ProjectId";

        return await dbConnection.QuerySingleOrDefaultAsync<Guid>(sql, new { ProjectId = projectId }, transaction);
    }

    /// <inheritdoc />
    public async Task<ProjectCategory> GetCategoryByIdAsync(Guid categoryId)
    {
        var query = $@"SELECT * FROM {EntityMapper.TbName<ProjectCategory>()} 
                        WHERE {EntityMapper.ColName<ProjectCategory>(x => x.Id)} = @Id";

        var category = await dbConnection.QuerySingleOrDefaultAsync<ProjectCategory>(query, new { Id = categoryId });
        if (category is null)
        {
            throw new EntityNotFoundException("Категория не найдена");
        }

        return category;
    }

    /// <inheritdoc />
    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var query = $@"DELETE FROM {EntityMapper.TbName<ProjectCategory>()} 
                        WHERE {EntityMapper.ColName<ProjectCategory>(x => x.Id)} = @Id";

        var deleted = await dbConnection.ExecuteAsync(query, new { Id = categoryId });
        if (deleted == 0)
        {
            throw new EntityNotFoundException("Категория не найдена");
        }
    }

    /// <inheritdoc />
    public async Task<Guid> AddCategoryAsync(string name, string description)
    {
        var query = $@"INSERT INTO {EntityMapper.TbName<ProjectCategory>()} 
                        VALUES (@Id, @Name, @Description, @IsActive, @IsSystem, @CreatedAt, @UpdatedAt)";

        var categoryId = Guid.NewGuid();
        await dbConnection.ExecuteAsync(query, new
        {
            Id = categoryId,
            Name = name,
            Description = description,
            IsActive = true,
            IsSystem = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = (DateTime?)null
        });

        return categoryId;
    }

    /// <inheritdoc />
    public async Task<ProjectCategory> UpdateCategoryAsync(Guid categoryId, string? name, string? description, bool? isActive = true)
    {
        var updates = new List<string>();
        var parameters = new DynamicParameters();

        parameters.Add(nameof(ProjectCategory.Id), categoryId);

        if (name is not null)
        {
            updates.Add($"{EntityMapper.ColName<ProjectCategory>(x => x.Name)} = @Name");
            parameters.Add(nameof(ProjectCategory.Name), name);
        }

        if (description is not null)
        {
            updates.Add($"{EntityMapper.ColName<ProjectCategory>(x => x.Description)} = @Description");
            parameters.Add(nameof(ProjectCategory.Description), description);
        }

        updates.Add($"{EntityMapper.ColName<ProjectCategory>(x => x.IsActive)} = @IsActive");
        parameters.Add(nameof(ProjectCategory.IsActive), isActive);

        updates.Add($"{EntityMapper.ColName<ProjectCategory>(x => x.UpdatedAt)} = @UpdatedAt");
        parameters.Add(nameof(ProjectCategory.UpdatedAt), DateTime.UtcNow);

        if (updates.Count == 0)
        {
            return await GetCategoryByIdAsync(categoryId);
        }

        var query = $@"UPDATE {EntityMapper.TbName<ProjectCategory>()}
                       SET {string.Join(", ", updates)}
                       WHERE {EntityMapper.ColName<ProjectCategory>(x => x.Id)} = @Id;

                       SELECT *
                       FROM {EntityMapper.TbName<ProjectCategory>()}
                       WHERE {EntityMapper.ColName<ProjectCategory>(x => x.Id)} = @Id;";

        var category = await dbConnection.QuerySingleAsync<ProjectCategory>(query, parameters);
        return category;
    }
}