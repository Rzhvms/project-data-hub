namespace Application.UseCases.Categories.Dto.Request;

public record AddCategoryRequest
{
    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Описание категории
    /// </summary>
    public required string Description { get; set; }
}