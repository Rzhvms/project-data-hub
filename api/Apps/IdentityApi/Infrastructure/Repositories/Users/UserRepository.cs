using System.Data;
using Application.Ports.Repositories;
using CoreLib.Database.DapperExtensions.EntityMapper;
using CoreLib.Exceptions;
using Dapper;
using Domain.Entities.IdentityUser;

namespace Infrastructure.Repositories.Users;

/// <inheritdoc />
public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> GetUserByEmailOrUsernameAsync(string email, string username)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<User>()}
                     WHERE {EntityMapper.ColName<User>(x => x.Email)} = @Email
                     OR {EntityMapper.ColName<User>(x => x.Username)} = @Username";

        var user = await dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email, Username = username });

        if (user is not null)
        {
            var getTokenSql = $@"SELECT * FROM {EntityMapper.TbName<RefreshToken>()} 
                                 WHERE {EntityMapper.ColName<RefreshToken>(x => x.UserId)} = @UserId";

            user.RefreshToken = await dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(
                getTokenSql, new { UserId = user.Id });
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetUserByEmailOrUsernameAsync(string identifier)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<User>()}
                     WHERE {EntityMapper.ColName<User>(x => x.Email)} = @Email
                     OR {EntityMapper.ColName<User>(x => x.Username)} = @Username";

        var user = await dbConnection.QueryFirstOrDefaultAsync<User>(sql,
            new { Email = identifier, Username = identifier });

        if (user is not null)
        {
            var getTokenSql = $@"SELECT * FROM {EntityMapper.TbName<RefreshToken>()} 
                                 WHERE {EntityMapper.ColName<RefreshToken>(x => x.UserId)} = @UserId";

            user.RefreshToken = await dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(
                getTokenSql, new { UserId = user.Id });
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetUserByUserIdAsync(Guid id)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<User>()}
                    WHERE {EntityMapper.ColName<User>(x => x.Id)} = @id";

        var user = await dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });

        if (user is not null)
        {
            var getTokenSql = $@"SELECT * FROM {EntityMapper.TbName<RefreshToken>()} 
                                 WHERE {EntityMapper.ColName<RefreshToken>(x => x.UserId)} = @UserId";

            user.RefreshToken = await dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(
                getTokenSql, new { UserId = user.Id });
        }

        return user;
    }

    /// <inheritdoc />
    public async Task CreateUserAsync(User user)
    {
        using var transaction = dbConnection.BeginTransaction();

        try
        {
            var insertUserSql = $@"INSERT INTO {EntityMapper.TbName<User>()}
                     VALUES (@Id, @Username, @Email, @Phone, @Password, @HashSalt, @IsEmailConfirmed, @FirstName, @LastName, @RoleId, @CreatedAt, @UpdatedAt)";

            await dbConnection.ExecuteAsync(insertUserSql, new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Phone,
                user.Password,
                user.HashSalt,
                user.IsEmailConfirmed,
                user.FirstName,
                user.LastName,
                user.RoleId,
                user.CreatedAt,
                user.UpdatedAt
            }, transaction);

            if (user.RefreshToken != null)
            {
                if (user.RefreshToken.Id == Guid.Empty)
                {
                    user.RefreshToken.Id = Guid.NewGuid();
                }

                user.RefreshToken.UserId = user.Id;

                var tokenSql = $@"INSERT INTO {EntityMapper.TbName<RefreshToken>()}
                              VALUES (@Id, @UserId, @Value, @Active, @ExpirationDate)";

                await dbConnection.ExecuteAsync(tokenSql, user.RefreshToken, transaction);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateUserAsync(User user)
    {
        var transaction = dbConnection.BeginTransaction();

        try
        {
            var sql = $@"
            UPDATE {EntityMapper.TbName<User>()}
            SET {EntityMapper.ColName<User>(x => x.Username)} = @Username,
                {EntityMapper.ColName<User>(x => x.Email)} = @Email,
                {EntityMapper.ColName<User>(x => x.Phone)} = @Phone,
                {EntityMapper.ColName<User>(x => x.Password)} = @Password,
                {EntityMapper.ColName<User>(x => x.HashSalt)} = @HashSalt,
                {EntityMapper.ColName<User>(x => x.FirstName)} = @FirstName,
                {EntityMapper.ColName<User>(x => x.LastName)} = @LastName,
                {EntityMapper.ColName<User>(x => x.IsEmailConfirmed)} = @IsEmailConfirmed,
                {EntityMapper.ColName<User>(x => x.RoleId)} = @RoleId,
                {EntityMapper.ColName<User>(x => x.UpdatedAt)} = @UpdatedAt
            WHERE {EntityMapper.ColName<User>(x => x.Id)} = @Id";

            await dbConnection.ExecuteAsync(sql, new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Phone,
                user.Password,
                user.HashSalt,
                user.FirstName,
                user.LastName,
                user.IsEmailConfirmed,
                user.RoleId,
                UpdatedAt = DateTime.UtcNow
            }, transaction);

            if (user.RefreshToken != null)
            {
                if (user.RefreshToken.Id == Guid.Empty)
                {
                    user.RefreshToken.Id = Guid.NewGuid();
                }

                user.RefreshToken.UserId = user.Id;

                var tokenSql = $@"
                UPDATE {EntityMapper.TbName<RefreshToken>()}
                SET {EntityMapper.ColName<RefreshToken>(x => x.Value)} = @Value,
                    {EntityMapper.ColName<RefreshToken>(x => x.Active)} = @Active,
                    {EntityMapper.ColName<RefreshToken>(x => x.ExpirationDate)} = @ExpirationDate
                WHERE {EntityMapper.ColName<RefreshToken>(x => x.UserId)} = @UserId";

                await dbConnection.ExecuteAsync(tokenSql, new
                {
                    user.RefreshToken.Value,
                    user.RefreshToken.Active,
                    user.RefreshToken.ExpirationDate,
                    UserId = user.Id
                }, transaction);
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<User>()}";
        var users = await dbConnection.QueryAsync<User>(sql);
        return users;
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetPagedUsersAsync(int page, int pageSize)
    {
        var sql = $@"SELECT * FROM {EntityMapper.TbName<User>()} 
                 ORDER BY {EntityMapper.ColName<User>(x => x.CreatedAt)} DESC
                 OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        return await dbConnection.QueryAsync<User>(sql, new 
        { 
            Offset = (page - 1) * pageSize, 
            PageSize = pageSize 
        });
    }

    /// <inheritdoc />
    public async Task ChangeUserPasswordAsync(Guid id, string password, string hashSalt)
    {
        var sql = $@"
            UPDATE {EntityMapper.TbName<User>()}
            SET {EntityMapper.ColName<User>(x => x.Password)} = @Password,
                {EntityMapper.ColName<User>(x => x.HashSalt)} = @HashSalt,
                {EntityMapper.ColName<User>(x => x.UpdatedAt)} = @UpdatedAt
            WHERE {EntityMapper.ColName<User>(x => x.Id)} = @Id";
        
        await dbConnection.ExecuteAsync(sql, new { Id = id, Password = password, HashSalt = hashSalt, UpdatedAt = DateTime.UtcNow });
    }

    /// <inheritdoc />
    public async Task DeleteUserByIdAsync(Guid id)
    {
        var sql = $@"DELETE FROM {EntityMapper.TbName<User>()} WHERE {EntityMapper.ColName<User>(x => x.Id)} = @Id";
        var deleted = await dbConnection.ExecuteAsync(sql, new { Id = id });
        if (deleted == 0)
        {
            throw new EntityNotFoundException($"Пользователь с идентификатором {id} не найден");
        }
    }
}