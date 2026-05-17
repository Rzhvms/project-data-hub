namespace Application.UseCases.Categories.Dto.Request;

public record PatchCategoryRequest
{
    /// <summary>
    /// Отображаемое название категории
    /// </summary>
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Описание категории
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Признак активности категории
    /// </summary>
    public bool? IsActive { get; set; } = true;
}