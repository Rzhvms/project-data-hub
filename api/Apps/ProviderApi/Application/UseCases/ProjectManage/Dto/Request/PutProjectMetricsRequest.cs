using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Application.UseCases.ProjectManage.Dto.Request;

public record PutProjectMetricsRequest
{
    /// <summary>
    /// Общая площадь объекта.
    /// </summary>
    [JsonPropertyName("totalArea")]
    public decimal? TotalArea { get; init; }

    /// <summary>
    /// Площадь участка.
    /// </summary>
    [JsonPropertyName("siteArea")]
    public decimal? SiteArea { get; init; }

    /// <summary>
    /// Площадь застройки.
    /// </summary>
    [JsonPropertyName("buildingArea")]
    public decimal? BuildingArea { get; init; }

    /// <summary>
    /// Количество корпусов или секций.
    /// </summary>
    [JsonPropertyName("buildingsCount")]
    public int? BuildingsCount { get; init; }

    /// <summary>
    /// Этажность объекта.
    /// </summary>
    [JsonPropertyName("floors")]
    public int? Floors { get; init; }

    /// <summary>
    /// Количество квартир или помещений.
    /// </summary>
    [JsonPropertyName("apartmentsCount")]
    public int? ApartmentsCount { get; init; }

    /// <summary>
    /// Количество парковочных мест.
    /// </summary>
    [JsonPropertyName("parkingSpacesCount")]
    public int? ParkingSpacesCount { get; init; }
    
    /// <summary>
    /// Дополнительные данные, позволяющие расширить модель
    /// </summary>
    [JsonPropertyName("jsonData")]
    public JsonObject? JsonData { get; init; }
}