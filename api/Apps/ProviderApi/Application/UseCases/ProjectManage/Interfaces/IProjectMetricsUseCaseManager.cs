using Application.UseCases.ProjectManage.Dto.Request;
using Application.UseCases.ProjectManage.Dto.Response;

namespace Application.UseCases.ProjectManage.Interfaces;

/// <summary>
/// UseCase сценарии управления данными технико-экономических показателей объекта.
/// </summary>
public interface IProjectMetricsUseCaseManager
{
    /// <summary>
    /// Получить метрики по идентификатору проекта
    /// </summary>
    Task<GetProjectMetricsResponse> GetMetricsByProjectIdAsync(Guid projectId);

    /// <summary>
    /// Добавить метрики к проекту
    /// </summary>
    /// <returns>Идентификатор проекта!</returns>
    Task<AddProjectMetricsResponse> AddProjectMetricsAsync(Guid projectId, AddProjectMetricsRequest metrics);

    /// <summary>
    /// Удалить метрики по идентификатру проекта
    /// </summary>
    Task DeleteMetricsByProjectIdAsync(Guid projectId);

    /// <summary>
    /// Полностью обновить метрику по идентификатору проекта
    /// </summary>
    Task<PutProjectMetricsResponse> PutProjectMetricsByProjectId(Guid projectId, PutProjectMetricsRequest request);
}