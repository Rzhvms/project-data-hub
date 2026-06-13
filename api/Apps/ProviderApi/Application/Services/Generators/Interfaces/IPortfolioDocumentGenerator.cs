using Domain.Entities.Files;

namespace Application.Services.Generators.Interfaces;

/// <summary>
/// Генератор DOCX для портфолио
/// </summary>
public interface IPortfolioDocumentGenerator
{
    /// <summary>
    /// Генерирует DOCX-документ портфолио
    /// </summary>
    Task<byte[]> GenerateAsync(ProjectExportModel model);
}