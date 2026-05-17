using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.Project.Roles;
using Npgsql;

namespace Infrastructure.Repositories.Participants;

/// <inheritdoc />
internal class ParticipantRepository(IDbConnection dbConnection) : IParticipantRepository
{
    /// <inheritdoc />
    public async Task<List<ProjectParticipant>> GetAllParticipantsAsync()
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<ProjectParticipant>()}";
        var participants = await dbConnection.QueryAsync<ProjectParticipant>(sql);
        return participants.AsList();
    }

    /// <inheritdoc />
    public async Task AddProjectParticipantLink(Guid projectId, Guid participantId, IDbTransaction? transaction = null)
    {
        try
        {
            var projectCategoryLinkSql =
                $@"INSERT INTO {EntityMapper.TbName<ProjectParticipantLink>()} VALUES (@Id, @ProjectId, @ParticipantId)";

            await dbConnection.ExecuteAsync(projectCategoryLinkSql, new
            {
                Id = Guid.NewGuid(), ProjectId = projectId, ParticipantId = participantId
            }, transaction);
        }
        catch (PostgresException)
        {
            throw new EntityNotFoundException("Передан некорректный идентификатор роли");
        }
    }

    /// <inheritdoc />
    public async Task<bool> CheckExistProjectParticipantLink(Guid projectId, Guid participantId,
        IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<ProjectParticipantLink>()}
                    WHERE {EntityMapper.ColName<ProjectParticipantLink>(x => x.ProjectId)} = @ProjectId
                    AND {EntityMapper.ColName<ProjectParticipantLink>(x => x.ParticipantId)} = @ParticipantId";

        var res = await dbConnection.QueryAsync<ProjectParticipantLink>(sql, new
        {
            ProjectId = projectId, ParticipantId = participantId
        }, transaction);

        return res.Any();
    }

    /// <inheritdoc />
    public async Task<List<Guid>> GetParticipantIdListByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null)
    {
        var sql = $@"SELECT {EntityMapper.ColName<ProjectParticipantLink>(x => x.ParticipantId)} 
                    FROM {EntityMapper.TbName<ProjectParticipantLink>()} 
                    WHERE {EntityMapper.ColName<ProjectParticipantLink>(x => x.ProjectId)} = @ProjectId";

        var participants = await dbConnection.QueryAsync<Guid>(sql, new { ProjectId = projectId }, transaction);
        return participants.AsList();
    }

    /// <inheritdoc />
    public async Task<ProjectParticipant> GetParticipantByIdAsync(Guid participantId)
    {
        var query = $@"SELECT * FROM {EntityMapper.TbName<ProjectParticipant>()} 
                        WHERE {EntityMapper.ColName<ProjectParticipant>(x => x.Id)} = @Id";

        var participant = await dbConnection.QuerySingleOrDefaultAsync<ProjectParticipant>(query, new { Id = participantId });
        if (participant is null)
        {
            throw new EntityNotFoundException("Роль не найдена");
        }

        return participant;
    }

    /// <inheritdoc />
    public async Task DeleteParticipantAsync(Guid participantId)
    {
        var query = $@"DELETE FROM {EntityMapper.TbName<ProjectParticipant>()} 
                        WHERE {EntityMapper.ColName<ProjectParticipant>(x => x.Id)} = @Id";

        var deleted = await dbConnection.ExecuteAsync(query, new { Id = participantId });
        if (deleted == 0)
        {
            throw new EntityNotFoundException("Роль не найдена");
        }
    }

    /// <inheritdoc />
    public async Task<Guid> AddParticipantAsync(string name, string description)
    {
        var query = $@"INSERT INTO {EntityMapper.TbName<ProjectParticipant>()} 
                        VALUES (@Id, @Name, @Description, @IsActive, @IsSystem, @CreatedAt, @UpdatedAt)";

        var participantId = Guid.NewGuid();
        await dbConnection.ExecuteAsync(query, new
        {
            Id = participantId,
            Name = name,
            Description = description,
            IsActive = true,
            IsSystem = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = (DateTime?)null
        });

        return participantId;
    }

    /// <inheritdoc />
    public async Task<ProjectParticipant> UpdateParticipantAsync(Guid participantId, string? name, string? description, bool? isActive = true)
    {
        var updates = new List<string>();
        var parameters = new DynamicParameters();

        parameters.Add(nameof(ProjectParticipant.Id), participantId);

        if (name is not null)
        {
            updates.Add($"{EntityMapper.ColName<ProjectParticipant>(x => x.Name)} = @Name");
            parameters.Add(nameof(ProjectParticipant.Name), name);
        }

        if (description is not null)
        {
            updates.Add($"{EntityMapper.ColName<ProjectParticipant>(x => x.Description)} = @Description");
            parameters.Add(nameof(ProjectParticipant.Description), description);
        }

        updates.Add($"{EntityMapper.ColName<ProjectParticipant>(x => x.IsActive)} = @IsActive");
        parameters.Add(nameof(ProjectParticipant.IsActive), isActive);

        updates.Add($"{EntityMapper.ColName<ProjectParticipant>(x => x.UpdatedAt)} = @UpdatedAt");
        parameters.Add(nameof(ProjectParticipant.UpdatedAt), DateTime.UtcNow);

        if (updates.Count == 0)
        {
            return await GetParticipantByIdAsync(participantId);
        }

        var query = $@"UPDATE {EntityMapper.TbName<ProjectParticipant>()}
                       SET {string.Join(", ", updates)}
                       WHERE {EntityMapper.ColName<ProjectParticipant>(x => x.Id)} = @Id;

                       SELECT *
                       FROM {EntityMapper.TbName<ProjectParticipant>()}
                       WHERE {EntityMapper.ColName<ProjectParticipant>(x => x.Id)} = @Id;";

        var category = await dbConnection.QuerySingleAsync<ProjectParticipant>(query, parameters);
        return category;
    }
}