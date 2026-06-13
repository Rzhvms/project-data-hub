using Application.Ports.Repositories;
using Application.Services.Generators.Interfaces;
using Application.UseCases.ExportFiles.Dto.Request;
using Application.UseCases.ExportFiles.Dto.Response;
using Application.UseCases.ExportFiles.Interfaces;
using CoreLib.Exceptions;
using Domain.Entities.Files;

namespace Application.UseCases.ExportFiles;

/// <inheritdoc />
public class ExportFilesUseCaseManager(
    IProjectRepository projectRepository,
    IProjectMetricsRepository projectMetricsRepository,
    IImageRepository imageRepository,
    IPresentationDocumentGenerator presentationGenerator,
    IPortfolioDocumentGenerator portfolioGenerator) : IExportFilesUseCaseManager
{
    /// <inheritdoc />
    public async Task<FileExportResponse> ExportPresentationAsync(ExportPresentationRequest request)
    {
        var project = await projectRepository.GetFullProjectByIdAsync(request.ProjectId) ?? throw new EntityNotFoundException("Project not found");
        
        var metrics = await projectMetricsRepository.GetMetricsByProjectIdAsync(request.ProjectId);
        var images = await imageRepository.GetAllByProjectIdAsync(request.ProjectId);

        var model = new ProjectExportModel
        {
            Project = project,
            Metrics = metrics,
            Images = images.ToList().AsReadOnly()
        };

        var content = await presentationGenerator.GenerateAsync(model);

        return new FileExportResponse
        {
            FileName = $"{SafeFileName(project.Title)}_presentation.pptx",
            ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            Content = content
        };
    }

    /// <inheritdoc />
    public async Task<FileExportResponse> ExportPortfolioAsync(ExportPortfolioRequest request)
    {
        var project = await projectRepository.GetFullProjectByIdAsync(request.ProjectId) ?? throw new EntityNotFoundException("Project not found");

        var metrics = await projectMetricsRepository.GetMetricsByProjectIdAsync(request.ProjectId);
        var images = await imageRepository.GetAllByProjectIdAsync(request.ProjectId);

        var model = new ProjectExportModel
        {
            Project = project,
            Metrics = metrics,
            Images = images.ToList().AsReadOnly()
        };

        var content = await portfolioGenerator.GenerateAsync(model);

        return new FileExportResponse
        {
            FileName = $"{SafeFileName(project.Title)}_portfolio.docx",
            ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            Content = content
        };
    }

    private static string SafeFileName(string name)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');

        return name.Trim().Replace(' ', '_');
    }
}