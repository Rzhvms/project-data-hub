using System.Data;
using Domain.Entities.Project.Roles;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для ролей (участников) проекта
/// </summary>
public interface IParticipantRepository
{
    /// <summary>
    /// Получить всех участников
    /// </summary>
    Task<List<ProjectParticipant>> GetAllParticipantsAsync();
    
    /// <summary>
    /// Добавить связь проекта и участника
    /// </summary>
    Task AddProjectParticipantLink(Guid projectId, Guid participantId, IDbTransaction? transaction = null);
   
    /// <summary>
    /// Проверить, есть ли связь проекта и участника
    /// </summary>
    Task<bool> CheckExistProjectParticipantLink(Guid projectId, Guid participantId, IDbTransaction? transaction = null);
   
    /// <summary>
    /// Получить идентификатор участников проекта
    /// </summary>
    Task<List<Guid>> GetParticipantIdListByProjectIdAsync(Guid projectId, IDbTransaction? transaction = null);
    
    /// <summary>
    /// Получить участников по идентификатору
    /// </summary>
    Task<ProjectParticipant> GetParticipantByIdAsync(Guid participantId);
    
    /// <summary>
    /// Удалить участника по идентификатору
    /// </summary>
    Task DeleteParticipantAsync(Guid participantId);
    
    /// <summary>
    /// Добавить участника
    /// </summary>
    Task<Guid> AddParticipantAsync(string name, string description);
    
    /// <summary>
    /// Обновить данные участника
    /// </summary>
    Task<ProjectParticipant> UpdateParticipantAsync(Guid participantId, string? name, string? description, bool? isActive = true);
}