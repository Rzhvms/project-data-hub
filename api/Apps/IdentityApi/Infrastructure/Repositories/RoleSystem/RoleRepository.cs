using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using Dapper;
using Domain.Entities.RoleSystem;

namespace Infrastructure.Repositories.RoleSystem;

/// <inheritdoc/>
public class RoleRepository(IDbConnection dbConnection) : IRoleRepository
{
    /// <inheritdoc/>
    public async Task<Role> GetRoleByRoleCode(string roleCode)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<Role>()} 
                     WHERE {EntityMapper.ColName<Role>(x => x.RoleCode)} = @RoleCode";
        
        return await dbConnection.QuerySingleAsync<Role>(sql, new { RoleCode = roleCode });
    }
}