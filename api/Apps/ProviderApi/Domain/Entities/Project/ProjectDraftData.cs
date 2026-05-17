using System.Text.Json.Nodes;

namespace Domain.Entities.Project;

public sealed record ProjectDraftData
{
    public Guid? ProjectId { get; init; }
    public string? Title { get; init; }
    public string? ShortTitle { get; init; }
    public string? CityRegion { get; init; }
    public string? Address { get; init; }
    public string? DesignYearPeriod { get; init; }
    public int? RealizationYear { get; init; }
    public string? ProjectStatus { get; init; }
    public string? ObjectType { get; init; }
    public string? Customer { get; init; }
    public string? InpadRole { get; init; }
    public string? DesignStage { get; init; }
    public string? ShortDescription { get; init; }
    public string? LongDescription { get; init; }
    public string? Publisher { get; init; }
    public List<Guid>? CategoryIdList { get; init; }
    public List<Guid>? ParticipantIdList { get; init; }
    public JsonObject? ProjectMetrics { get; set; }
}