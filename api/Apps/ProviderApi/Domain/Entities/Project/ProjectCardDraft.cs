using System.Text.Json.Nodes;

namespace Domain.Entities.Project;

/// <summary>
/// Модель неопубликованной записи проекта (черновик)
/// </summary>
public record ProjectCardDraft
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; init; }
    
    /// <summary>
    /// Параметры полей в карточке проекта
    /// </summary>
    public JsonObject? ProjectData { get; init; }
}