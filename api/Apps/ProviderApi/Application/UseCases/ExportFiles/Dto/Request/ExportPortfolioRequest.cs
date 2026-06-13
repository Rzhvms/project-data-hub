namespace Application.UseCases.ExportFiles.Dto.Request;

public record ExportPortfolioRequest
{
    public Guid ProjectId { get; init; }
}