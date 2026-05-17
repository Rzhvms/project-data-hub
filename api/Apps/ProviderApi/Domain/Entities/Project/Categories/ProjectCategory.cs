namespace Domain.Entities.Project.Categories;

/// <summary>
/// Модель Категории проекта (объекта)
/// </summary>
public sealed record ProjectCategory
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Описание категории
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Признак активности категории
    /// </summary>
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Признак системной категории
    /// </summary>
    public bool IsSystem { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}
