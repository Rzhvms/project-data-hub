using System.Text.Json.Serialization;

namespace Application.UseCases.ProjectManage.Dto.Response;

public record GetSimpleProjectResponse
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    [JsonPropertyName("projectId")]
    public Guid Id { get; init; }
    
    /// <summary>
    /// Название проекта
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }
    
    /// <summary>
    /// Город / Регион, в котором расположен объект
    /// </summary>
    [JsonPropertyName("place")]
    public string? Place { get; init; }
    
    /// <summary>
    /// Тип объекта, например: жилой комплекс, общественное здание, благоустройство
    /// </summary>
    [JsonPropertyName("objectType")]
    public string? ObjectType { get; init; }
    
    /// <summary>
    /// Статус проекта, например: концепция, проектная документация, реализован, в строительстве
    /// </summary>
    [JsonPropertyName("projectStatus")]
    public string? ProjectStatus { get; init; }
    
    /// <summary>
    /// Дата создания записи.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Дата изменения записи.
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }
    
    /// <summary>
    /// Автор, ответственный (ФИО)
    /// </summary>
    [JsonPropertyName("publisher")]
    public string? Publisher { get; init; }
    
    /// <summary>
    /// Статус публикации проекта
    /// </summary>
    [JsonPropertyName("publicationStatus")]
    public string? PublicationStatus { get; init; }
}