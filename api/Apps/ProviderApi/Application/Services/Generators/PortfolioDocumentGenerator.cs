using Application.Ports.Storages;
using Application.Services.Generators.Interfaces;
using CoreLib.Exceptions;
using Domain.Entities.Files;
using Microsoft.Extensions.Hosting;
using MiniSoftware;

namespace Application.Services.Generators;

internal sealed class PortfolioDocumentGenerator(IMinioFileReader minioFileReader, IHostEnvironment environment)
    : IPortfolioDocumentGenerator
{
    public async Task<byte[]> GenerateAsync(ProjectExportModel model)
    {
        var templatePath = Path.Combine(environment.ContentRootPath, "Templates", "portfolio-base.docx");

        if (!File.Exists(templatePath))
            throw new EntityNotFoundException($"DOCX template not found {templatePath}");

        // Ищем главную картинку и подготавливаем объект MiniWordPicture
        var mainImageFile = model.Images.FirstOrDefault(x => x.IsMain);
        MiniWordPicture? mainMiniWordImage = null;
        
        if (mainImageFile?.ObjectPath != null)
        {
            var bytes = await minioFileReader.ReadAllBytesAsync(mainImageFile.ObjectPath);
            mainMiniWordImage = new MiniWordPicture 
            { 
                Bytes = bytes, 
                Width = 600,
                Height = 400,
                Extension = GetExtension(mainImageFile.ObjectPath)
            };
        }

        // Формируем словарь данных
        var value = new Dictionary<string, object>
        {
            // Текстовые поля
            ["title"] = model.Project.Title,
            ["shortTitle"] = model.Project.ShortTitle ?? "---",
            ["cityRegion"] = model.Project.CityRegion,
            ["address"] = model.Project.Address ?? "-",
            ["designPeriod"] = model.Project.DesignYearPeriod ?? "-",
            ["designStage"] = model.Project.DesignStage ?? "-",
            ["realizationYear"] = model.Project.RealizationYear ?? "-",
            ["projectStatus"] = model.Project.ProjectStatus,
            ["publicationStatus"] = model.Project.PublicationStatus.ToString(),
            ["objectType"] = model.Project.ObjectType,
            ["customer"] = model.Project.Customer ?? "-",
            ["shortDescription"] = model.Project.ShortDescription,
            ["longDescription"] = model.Project.LongDescription ?? "-",
            ["inpadRole"] = model.Project.InpadRole,
            ["publisher"] = model.Project.Publisher ?? "-",
            
            
            // ТЭПы
            ["totalArea"] = model.Metrics?.TotalArea?.ToString() ?? "-",
            ["siteArea"] = model.Metrics?.SiteArea?.ToString() ?? "-",
            ["buildingArea"] = model.Metrics?.SiteArea?.ToString() ?? "-",
            ["buildingsCount"] = model.Metrics?.BuildingsCount?.ToString() ?? "-",
            ["floors"] = model.Metrics?.Floors?.ToString() ?? "-",
            ["apartmentsCount"] = model.Metrics?.ApartmentsCount?.ToString() ?? "-",
            ["parkingSpacesCount"] = model.Metrics?.ParkingSpacesCount?.ToString() ?? "-",
            
            // Список картинок (MiniWord найдет переменные в таблице и размножит строки)
            ["images"] = model.Images
                .Where(x => x.UseInPortfolio || x.IsMain)
                .Take(5)
                .Select(x => new 
                {
                    imgTitle = string.IsNullOrWhiteSpace(x.Title) ? "Без названия" : x.Title,
                    imgDesc = string.IsNullOrWhiteSpace(x.Description) ? "Без описания" : x.Description
                }).ToList()
        };

        // Подставляем главную картинку. Если ее, нет очищаем тег
        if (mainMiniWordImage != null)
        {
            value["mainImage"] = mainMiniWordImage;
        }
        else
        {
            value["mainImage"] = "";
        }

        // Генерируем документ сразу в MemoryStream
        using var memoryStream = new MemoryStream();
        
        // MiniWord забиндит все данные, включая байты изображений и списки
        MiniWord.SaveAsByTemplate(memoryStream, templatePath, value);
        
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Вспомогательный метод для определения формата картинки
    /// </summary>
    private static string GetExtension(string path)
    {
        var ext = Path.GetExtension(path).TrimStart('.').ToLower();
        return ext is "jpg" or "jpeg" or "png" ? ext : "jpeg";
    }
}