namespace Application.UseCases.Participants.Dto.Response;

public record PatchParticipantResponse
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Описание участника
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Признак активности участника
    /// </summary>
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Признак системного участника
    /// </summary>
    public bool IsSystem { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}