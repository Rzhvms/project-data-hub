namespace Domain.Entities.Project.Roles;

/// <summary>
/// Справочник участников проекта и их роль.
/// </summary>
public sealed record ProjectParticipant
{
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название роли для отображения в интерфейсе.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Описание роли.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Признак активности роли.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Признак системной роли.
    /// </summary>
    public bool IsSystem { get; init; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего изменения.
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}