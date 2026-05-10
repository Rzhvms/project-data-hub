namespace Application.UseCases.ProjectManage.Dto.Request;

public record PublicateProjectRequest
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; init; }
}