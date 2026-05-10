namespace Domain.Entities.Project.Categories;

/// <summary>
/// Модель Категории проекта (объекта)
/// </summary>
public sealed record ProjectCategory
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание категории
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Порядок сортировки в интерфейсе
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Признак активности категории
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Признак системной категории
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
