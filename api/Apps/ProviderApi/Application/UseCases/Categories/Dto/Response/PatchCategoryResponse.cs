using System.Text.Json.Serialization;

namespace Application.UseCases.Categories.Dto.Response;

public record PatchCategoryResponse
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Описание категории
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// Признак активности категории
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Признак системной категории
    /// </summary>
    [JsonPropertyName("isSystem")]
    public bool IsSystem { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }
}