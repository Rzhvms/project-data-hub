using Application.Ports.Repositories;
using Application.UseCases.Images.Dto.Request;
using Application.UseCases.Images.Dto.Response;
using Application.UseCases.Images.Interfaces;
using CoreLib.Exceptions;
using Domain.Entities.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Application.UseCases.Images;

/// <inheritdoc />
internal class ImageUseCase(IProjectRepository projectRepository, IImageRepository imageRepository, IMinioClient minio, IConfiguration configuration) : IImageUseCase
{
    private readonly string _bucket = configuration["Minio:Bucket"]!;
    private readonly string _publicUrl = configuration["Minio:PublicUrl"]!;
    
    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/svg",
        "image/webp"
    ];
    
    /// <inheritdoc />
    public async Task AddProjectImageAsync(Guid projectId, UploadProjectImageRequest request)
    {
        ValidateFile(request.File); 
        
        var project = await projectRepository.GetFullProjectByIdAsync(projectId);
        
        if (project is null) throw new EntityNotFoundException("Project not found");

        var fileName = Guid.NewGuid() + Path.GetExtension(request.File.FileName);
        var objectPath = $"{projectId}/{fileName}";
        try
        {
            // Пытаемся загрузить файл в MinIO
            await minio.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectPath)
                    .WithStreamData(request.File.OpenReadStream())
                    .WithObjectSize(request.File.Length)
                    .WithContentType(request.File.ContentType));
            
            if (request.IsMain)
            {
                var existingMain = await imageRepository.GetMainImageAsync(projectId);
                
                if (existingMain is not null)
                {
                    existingMain.IsMain = false;
                    await imageRepository.UpdateAsync(existingMain);
                }
            }

            var image = new ProjectImage
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                ObjectPath = objectPath,
                Title = request.Title,
                Description = request.Description,
                AlternativeText = request.AlternativeText,
                UseInSite = request.UseInSite,
                UseInPortfolio = request.UseInPortfolio,
                UseInPresentation = request.UseInPresentation,
                IsMain = request.IsMain,
            };

            await imageRepository.CreateAsync(image);
        }
        catch (Exception)
        {
            await minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(_bucket).WithObject(objectPath));
            throw new InvalidObjectException("Произошла ошибка при загрузке изображения");
        }
    }

    /// <inheritdoc />
    public async Task DeleteProjectImageAsync(Guid projectId, Guid imageId)
    {
        var image = await imageRepository.GetByIdAsync(imageId);

        // Удаление файла из MinIO
        await minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(_bucket).WithObject(image.ObjectPath));
        await imageRepository.DeleteAsync(projectId, imageId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProjectImageResponse>> GetProjectImagesAsync(Guid projectId)
    {
        var images = await imageRepository.GetAllByProjectIdAsync(projectId);
        
        var response = images.Select(image => new ProjectImageResponse {
            Id = image.Id,
            Url = $"{_publicUrl}/{image.ObjectPath}",
            ProjectId = image.ProjectId,
            Title = image.Title,
            Description = image.Description,
            AlternativeText = image.AlternativeText,
            UseInSite = image.UseInSite,
            UseInPortfolio = image.UseInPortfolio,
            UseInPresentation = image.UseInPresentation,
            IsMain = image.IsMain
        });
        
        return response;
    }

    /// <summary>
    /// Валидация полученного на загрузку файла
    /// </summary>
    private static void ValidateFile(IFormFile file)
    {
        if (file == null) throw new InvalidObjectException("Файл не передан");
        if (file.Length == 0) throw new InvalidObjectException("Файл пустой");
        if (file.Length > 5_000_000) throw new InvalidObjectException("Размер файла превышает 5MB");
        if (!AllowedContentTypes.Contains(file.ContentType)) throw new InvalidObjectException("Недопустимый тип файла");
    }
}