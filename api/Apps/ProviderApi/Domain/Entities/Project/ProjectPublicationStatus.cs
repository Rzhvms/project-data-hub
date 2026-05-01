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
    /// На проверке
    /// </summary>
    OnReview = 2,
    
    /// <summary>
    /// Требуется доработка
    /// </summary>
    ForRevision = 3,
    
    /// <summary>
    /// Опубликован
    /// </summary>
    Published = 4,
    
    /// <summary>
    /// Архивный
    /// </summary>
    Archived = 5,
    
    /// <summary>
    /// Ошибка публикации
    /// </summary>
    PublicationError = 6
}