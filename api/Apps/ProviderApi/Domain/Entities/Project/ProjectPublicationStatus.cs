namespace Domain.Entities.Project;

/// <summary>
/// Статус публикации объекта
/// </summary>
public enum ProjectPublicationStatus
{
    /// <summary>
    /// Черновик
    /// </summary>
    Draft = 1,
    
    /// <summary>
    /// Опубликован
    /// </summary>
    Published = 4,
    
    /// <summary>
    /// Архивный
    /// </summary>
    Archived = 5,
}