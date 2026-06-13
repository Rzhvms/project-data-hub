namespace Application.UseCases.ExportFiles.Dto.Request;

public record ExportPresentationRequest
{
    public Guid ProjectId { get; init; }
}