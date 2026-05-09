namespace Domain.Entities.Project.Roles;

/// <summary>
/// Справочник участников проекта и их роль.
/// </summary>
public sealed record ProjectParticipant
{
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название роли для отображения в интерфейсе.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание роли.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Порядок сортировки в интерфейсе.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Признак активности роли.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Признак системной роли.
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего изменения.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}