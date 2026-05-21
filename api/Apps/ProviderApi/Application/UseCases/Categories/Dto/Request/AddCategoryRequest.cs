using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Categories.Dto.Request;

public record AddCategoryRequest
{
    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    [MaxLength(200)]
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Описание категории
    /// </summary>
    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}