namespace Application.UseCases.Participants.Dto.Request;

public record PatchParticipantRequest
{
    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Описание участника
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Признак активности участника
    /// </summary>
    public bool? IsActive { get; set; } = true;
}