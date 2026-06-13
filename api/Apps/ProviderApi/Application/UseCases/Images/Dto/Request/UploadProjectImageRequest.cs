using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Images.Dto.Request;

public record UploadProjectImageRequest
{
    [Required]
    public required IFormFile File { get; init; }
    
    [Required]
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    
    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; init; }
    
    [JsonPropertyName("alternativeText")]
    public string? AlternativeText { get; init; }
    
    [Required]
    [JsonPropertyName("useInSite")]
    public required bool UseInSite { get; init; }
    
    [Required]
    [JsonPropertyName("useInPresentation")]
    public required bool UseInPresentation { get; init; }
    
    [Required]
    [JsonPropertyName("useInPortfolio")]
    public required bool UseInPortfolio { get; init; }

    [Required]
    [JsonPropertyName("isMain")] 
    public required bool IsMain { get; init; } = false;
}