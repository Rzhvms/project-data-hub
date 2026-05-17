namespace Application.UseCases.Participants.Dto.Response;

public record GetParticipantResponse
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание участника
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Признак активности участника
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Признак системного участника
    /// </summary>
    public bool IsSystem { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}