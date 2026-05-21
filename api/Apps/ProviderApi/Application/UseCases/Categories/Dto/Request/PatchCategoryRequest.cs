using System.Text.Json.Serialization;

namespace Application.UseCases.Categories.Dto.Request;

public record PatchCategoryRequest
{
    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Описание категории
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Признак активности категории
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; } = true;
}