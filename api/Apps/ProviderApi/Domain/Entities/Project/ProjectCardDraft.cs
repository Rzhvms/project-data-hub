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
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Параметры полей в карточке проекта
    /// </summary>
    public JsonObject? ProjectData { get; set; }
}