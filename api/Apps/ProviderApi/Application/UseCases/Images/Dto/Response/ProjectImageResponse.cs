namespace Application.UseCases.Images.Dto.Response;

public record ProjectImageResponse
{
    /// <summary>
    /// Идентификатор изображения
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Ccылка
    /// </summary>
    public string? Url { get; init; }
    
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; init; }
    
    /// <summary>
    /// Заголовок
    /// </summary>
    public string? Title { get; init; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Альтернативное описание
    /// </summary>
    public string? AlternativeText { get; init; }
    
    /// <summary>
    /// Использовать на сайте
    /// </summary>
    public bool UseInSite { get; init; }
    
    /// <summary>
    /// Использовать в презентации
    /// </summary>
    public bool UseInPresentation { get; init; }
    
    /// <summary>
    /// Использовать в портфолио
    /// </summary>
    public bool UseInPortfolio { get; init; }
}