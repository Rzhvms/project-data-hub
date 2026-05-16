namespace Application.UseCases.Categories.Dto.Response;

public record GetCategoryListResponse
{
    public List<GetCategoryResponse> CategoryList { get; init; }
}