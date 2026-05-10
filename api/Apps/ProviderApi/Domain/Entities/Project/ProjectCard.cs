using Domain.Entities.Project.Categories;
using Domain.Entities.Project.Roles;

namespace Domain.Entities.Project;

/// <summary>
/// Карточка объекта компании.
/// Основная агрегирующая сущность, содержащая ключевые данные объекта,
/// описание, характеристики, SEO и связанные блоки.
/// </summary>
public sealed record ProjectCard
{
    /// <summary>
    /// Уникальный идентификатор карточки объекта.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Полное публичное название объекта.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Краткое название объекта для списков, карточек и презентаций.
    /// </summary>
    public string? ShortTitle { get; init; }

    /// <summary>
    /// Город / Регион, в котором расположен объект.
    /// </summary>
    public string CityRegion { get; init; } = null!;

    /// <summary>
    /// Точный или ориентировочный адрес объекта.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Год / Период проектирования объекта.
    /// </summary>
    public string? DesignYearPeriod { get; init; }

    /// <summary>
    /// Год реализации объекта.
    /// </summary>
    public int? RealizationYear { get; init; }

    /// <summary>
    /// Статус проекта, например: концепция, проектная документация, реализован, в строительстве.
    /// </summary>
    public string ProjectStatus { get; init; } = null!;

    /// <summary>
    /// Тип объекта, например: жилой комплекс, общественное здание, благоустройство.
    /// </summary>
    public string ObjectType { get; init; } = null!;

    /// <summary>
    /// Заказчик объекта.
    /// </summary>
    public string? Customer { get; init; }

    /// <summary>
    /// Роль компании ИНПАД в проекте.
    /// </summary>
    public string InpadRole { get; init; } = null!;

    /// <summary>
    /// Стадия проектирования, например: П, Р, концепция, эскизный проект.
    /// </summary>
    public string? DesignStage { get; init; }

    /// <summary>
    /// Краткое описание объекта.
    /// </summary>
    public string ShortDescription { get; init; } = null!;

    /// <summary>
    /// Полное описание объекта.
    /// </summary>
    public string? LongDescription { get; init; }
    
    /// <summary>
    /// Статус публикации проекта.
    /// </summary>
    public ProjectPublicationStatus PublicationStatus { get; init; }
    
    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Дата изменения записи.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
    
    /// <summary>
    /// Автор, ответственный (ФИО)
    /// </summary>
    public string? Publisher { get; init; }

    /// <summary>
    /// Набор технико-экономических показателей объекта.
    /// </summary>
    public ProjectMetrics? Metrics { get; init; }

    /// <summary>
    /// Привязанные категории объекта.
    /// </summary>
    public List<ProjectCategoryLink> Categories { get; init; } = [];

    /// <summary>
    /// Участники проекта.
    /// </summary>
    public List<ProjectParticipant> Participants { get; init; } = [];
}