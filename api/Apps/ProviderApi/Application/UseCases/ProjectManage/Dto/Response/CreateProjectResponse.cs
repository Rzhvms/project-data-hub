namespace Application.UseCases.ProjectManage.Dto.Response;

public record CreateProjectResponse
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; init; }
}