using System.Text.Json.Serialization;

namespace Application.UseCases.Categories.Dto.Response;

public record AddCategoryResponse
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    [JsonPropertyName("categoryId")]
    public Guid CategoryId { get; init; }
}