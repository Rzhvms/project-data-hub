using System.Data;
using Domain.Entities.Project;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для управления данными технико-экономических показателей объекта.
/// </summary>
public interface IProjectMetricsRepository
{
    /// <summary>
    /// Получить метрики по идентификатору проекта
    /// </summary>
    Task<ProjectMetrics> GetMetricsByProjectIdAsync(Guid projectId);

    /// <summary>
    /// Добавить метрики к проекту
    /// </summary>
    /// <returns>Идентификатор проекта!</returns>
    Task<Guid> AddProjectMetricsAsync(ProjectMetrics metrics, IDbTransaction? transaction = null);

    /// <summary>
    /// Удалить метрики по идентификатру проекта
    /// </summary>
    Task DeleteMetricsByProjectIdAsync(Guid projectId);

    /// <summary>
    /// Полностью обновить метрику по идентификатору проекта
    /// </summary>
    Task<ProjectMetrics> PutProjectMetricsByProjectId(ProjectMetrics metrics, IDbTransaction? transaction = null);
}