namespace CoreLib.Audit;

/// <summary>
/// Запись журнала аудита, фиксирующая действие пользователя над сущностью
/// </summary>
public record AuditLog
{
    /// <summary>
    /// Уникальный идентификатор записи
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя, совершившего действие. null для системных действий
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// Тип действия
    /// </summary>
    public string Action { get; init; } = null!;

    /// <summary>
    /// Тип целевой сущности
    /// </summary>
    public string EntityType { get; init; } = null!;

    /// <summary>
    /// Идентификатор целевой сущности
    /// </summary>
    public Guid? EntityId { get; init; }

    /// <summary>
    /// Дополнительные данные в формате JSON
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Время совершения действия
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
