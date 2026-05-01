namespace Domain.Entities.Project.Categories;

/// <summary>
/// Связь объекта с категорией.
/// </summary>
public sealed record ProjectCategoryLink
{
    /// <summary>
    /// Идентификатор записи.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; set; }
}