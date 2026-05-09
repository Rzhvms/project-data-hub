namespace Domain.Entities.Project;

/// <summary>
/// SEO-параметры страницы объекта.
/// </summary>
public sealed record ProjectSeo
{
    /// <summary>
    /// SEO-заголовок страницы.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Meta description страницы.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// ЧПУ-адрес страницы.
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Ключевые слова, если используются.
    /// </summary>
    public string? Keywords { get; set; }
    
    /// <summary>
    /// Идентификатор изображения для превью в социальных сетях.
    /// </summary>
    public Guid? PreviewImageId { get; set; }
}