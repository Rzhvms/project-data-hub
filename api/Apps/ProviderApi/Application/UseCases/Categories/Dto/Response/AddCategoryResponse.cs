namespace Application.UseCases.Categories.Dto.Response;

public record AddCategoryResponse
{
    /// <summary>
    /// Идентификатор категории
    /// </summary>
    public Guid CategoryId { get; init; }
}