using System.Text.Json.Nodes;

namespace Domain.Entities.Project;

/// <summary>
/// Технико-экономические показатели объекта.
/// </summary>
public sealed record ProjectMetrics
{
    /// <summary>
    /// Идентификатор ТЭП
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    /// 
    public Guid ProjectId { get; init; }
    
    /// <summary>
    /// Общая площадь объекта.
    /// </summary>
    public decimal? TotalArea { get; init; }

    /// <summary>
    /// Площадь участка.
    /// </summary>
    public decimal? SiteArea { get; init; }

    /// <summary>
    /// Площадь застройки.
    /// </summary>
    public decimal? BuildingArea { get; init; }

    /// <summary>
    /// Количество корпусов или секций.
    /// </summary>
    public int? BuildingsCount { get; init; }

    /// <summary>
    /// Этажность объекта.
    /// </summary>
    public int? Floors { get; init; }

    /// <summary>
    /// Количество квартир или помещений.
    /// </summary>
    public int? ApartmentsCount { get; init; }

    /// <summary>
    /// Количество парковочных мест.
    /// </summary>
    public int? ParkingSpacesCount { get; init; }
    
    /// <summary>
    /// Дополнительные данные, позволяющие расширить модель
    /// </summary>
    public JsonObject? JsonData { get; init; }
}