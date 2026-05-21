using System.Text.Json.Serialization;

namespace Application.UseCases.Categories.Dto.Response;

public record GetCategoryListResponse
{
    [JsonPropertyName("categoryList")]
    public List<GetCategoryResponse>? CategoryList { get; init; }
}