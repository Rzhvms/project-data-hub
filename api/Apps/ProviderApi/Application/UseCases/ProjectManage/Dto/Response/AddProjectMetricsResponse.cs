namespace Application.UseCases.ProjectManage.Dto.Response;

public record AddProjectMetricsResponse
{
    public Guid ProjectId { get; init; }
}