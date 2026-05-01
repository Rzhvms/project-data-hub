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
    public Guid Id { get; set; }

    /// <summary>
    /// Полное публичное название объекта.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Краткое название объекта для списков, карточек и презентаций.
    /// </summary>
    public string? ShortTitle { get; set; }

    /// <summary>
    /// Город / Регион, в котором расположен объект.
    /// </summary>
    public string CityRegion { get; set; } = null!;

    /// <summary>
    /// Точный или ориентировочный адрес объекта.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Год / Период проектирования объекта.
    /// </summary>
    public string? DesignYearPeriod { get; set; }

    /// <summary>
    /// Год реализации объекта.
    /// </summary>
    public int? RealizationYear { get; set; }

    /// <summary>
    /// Статус проекта, например: концепция, проектная документация, реализован, в строительстве.
    /// </summary>
    public string ProjectStatus { get; set; } = null!;

    /// <summary>
    /// Тип объекта, например: жилой комплекс, общественное здание, благоустройство.
    /// </summary>
    public string ObjectType { get; set; } = null!;

    /// <summary>
    /// Заказчик объекта.
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Роль компании ИНПАД в проекте.
    /// </summary>
    public string InpadRole { get; set; } = null!;

    /// <summary>
    /// Стадия проектирования, например: П, Р, концепция, эскизный проект.
    /// </summary>
    public string? DesignStage { get; set; }

    /// <summary>
    /// Краткое описание объекта.
    /// </summary>
    public string ShortDescription { get; set; } = null!;

    /// <summary>
    /// Полное описание объекта.
    /// </summary>
    public string? LongDescription { get; set; }
    
    /// <summary>
    /// Статус публикации проекта.
    /// </summary>
    public ProjectPublicationStatus PublicationStatus { get; set; }
    
    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата изменения записи.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// SEO-блок карточки объекта.
    /// </summary>
    public ProjectSeo? Seo { get; set; }

    /// <summary>
    /// Набор технико-экономических показателей объекта.
    /// </summary>
    public ProjectMetrics? Metrics { get; set; }

    /// <summary>
    /// Привязанные категории объекта.
    /// </summary>
    public List<ProjectCategoryLink> Categories { get; set; } = [];

    /// <summary>
    /// Участники проекта.
    /// </summary>
    public List<ProjectParticipant> Participants { get; set; } = [];
}