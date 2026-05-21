namespace Domain.Entities.Project.Roles;

/// <summary>
/// Связь проекта с участниками проекта.
/// </summary>
public sealed record ProjectParticipantLink
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; init; }
    
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public Guid ParticipantId { get; init; }
}