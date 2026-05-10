namespace Application.UseCases.ProjectManage.Dto.Response;

public class GetSimpleProjectListResponse
{
    /// <summary>
    /// Список объектов для отображения на главной странице
    /// </summary>
    public required List<GetSimpleProjectResponse> ProjectList { get; init; }
}