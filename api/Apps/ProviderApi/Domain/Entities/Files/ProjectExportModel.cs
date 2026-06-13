using Domain.Entities.Project;

namespace Domain.Entities.Files;

/// <summary>
/// Сводная модель для генерации файлов экспорта
/// </summary>
public record ProjectExportModel
{
    /// <summary>
    /// Основные данные карточки проекта
    /// </summary>
    public required ProjectCard Project { get; init; }
    
    /// <summary>
    /// Технико-экономические показатели (могут быть не заполнены у проекта)
    /// </summary>
    public required ProjectMetrics? Metrics { get; init; }
    
    /// <summary>
    /// Выбранные для экспорта изображения
    /// </summary>
    public required IReadOnlyCollection<ProjectImage> Images { get; init; }
}