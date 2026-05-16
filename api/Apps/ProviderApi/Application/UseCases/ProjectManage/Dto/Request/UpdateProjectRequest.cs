namespace Application.UseCases.ProjectManage.Dto.Request;

public record UpdateProjectRequest
{
    /// <summary>
    /// Полное публичное название объекта.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Краткое название объекта для списков, карточек и презентаций.
    /// </summary>
    public string? ShortTitle { get; set; }

    /// <summary>
    /// Город / Регион, в котором расположен объект.
    /// </summary>
    public string? CityRegion { get; set; } = null!;

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
    public string? ProjectStatus { get; set; } = null!;

    /// <summary>
    /// Тип объекта, например: жилой комплекс, общественное здание, благоустройство.
    /// </summary>
    public string? ObjectType { get; set; } = null!;

    /// <summary>
    /// Заказчик объекта.
    /// </summary>
    public string? Customer { get; set; }

    /// <summary>
    /// Роль компании ИНПАД в проекте.
    /// </summary>
    public string? InpadRole { get; set; } = null!;

    /// <summary>
    /// Стадия проектирования, например: П, Р, концепция, эскизный проект.
    /// </summary>
    public string? DesignStage { get; set; }

    /// <summary>
    /// Краткое описание объекта.
    /// </summary>
    public string? ShortDescription { get; set; } = null!;

    /// <summary>
    /// Полное описание объекта.
    /// </summary>
    public string? LongDescription { get; set; }
    
    /// <summary>
    /// Автор, ответственный (ФИО)
    /// </summary>
    public string? Publisher { get; set; }
    
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid CategoryId { get; set; }
}