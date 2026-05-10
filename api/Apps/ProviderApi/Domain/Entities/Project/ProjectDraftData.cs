namespace Domain.Entities.Project;

public sealed record ProjectDraftData
{
    public string? Title { get; set; }
    public string? ShortTitle { get; set; }
    public string? CityRegion { get; set; }
    public string? Address { get; set; }
    public string? DesignYearPeriod { get; set; }
    public int? RealizationYear { get; set; }
    public string? ProjectStatus { get; set; }
    public string? ObjectType { get; set; }
    public string? Customer { get; set; }
    public string? InpadRole { get; set; }
    public string? DesignStage { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public string? Publisher { get; set; }
}