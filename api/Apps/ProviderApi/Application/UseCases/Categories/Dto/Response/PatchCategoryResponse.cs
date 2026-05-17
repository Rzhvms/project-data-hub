namespace Application.UseCases.Categories.Dto.Response;

public record PatchCategoryResponse
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    public string? Name { get; init; }

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