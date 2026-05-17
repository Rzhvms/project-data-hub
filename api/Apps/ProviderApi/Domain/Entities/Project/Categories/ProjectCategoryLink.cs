namespace Domain.Entities.Project.Categories;

/// <summary>
/// Связь объекта с категорией.
/// </summary>
public sealed record ProjectCategoryLink
{
    /// <summary>
    /// Идентификатор записи.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; init; }
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; init; }
}