using Domain.Entities.RoleSystem;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с системой ролей
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Получение роли по <see cref="Role.RoleCode"/>
    /// </summary>
    Task<Role> GetRoleByRoleCode(string roleCode);
    
    /// <summary>
    /// Получение роли по идентификатору.
    /// </summary>
    Task<Role> GetRoleByIdAsync(Guid id);
}