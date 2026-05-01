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
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    /// 
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Общая площадь объекта.
    /// </summary>
    public decimal? TotalArea { get; set; }

    /// <summary>
    /// Площадь участка.
    /// </summary>
    public decimal? SiteArea { get; set; }

    /// <summary>
    /// Площадь застройки.
    /// </summary>
    public decimal? BuildingArea { get; set; }

    /// <summary>
    /// Количество корпусов или секций.
    /// </summary>
    public int? BuildingsCount { get; set; }

    /// <summary>
    /// Этажность объекта.
    /// </summary>
    public int? Floors { get; set; }

    /// <summary>
    /// Количество квартир или помещений.
    /// </summary>
    public int? ApartmentsCount { get; set; }

    /// <summary>
    /// Количество парковочных мест.
    /// </summary>
    public int? ParkingSpacesCount { get; set; }
    
    /// <summary>
    /// Дополнительные данные, позволяющие расширить модель
    /// </summary>
    public JsonObject? JsonData { get; set; }
}