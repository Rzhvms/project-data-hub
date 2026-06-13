using Application.Ports.Storages;
using Application.Services.Generators.Interfaces;
using CoreLib.Exceptions;
using Domain.Entities.Files;
using Domain.Entities.Project;
using Microsoft.AspNetCore.Hosting;
using ShapeCrawler;

namespace Application.Services.Generators;

/// <inheritdoc />
internal sealed class PresentationDocumentGenerator(IMinioFileReader minioFileReader, IWebHostEnvironment environment)
    : IPresentationDocumentGenerator
{
    /// <inheritdoc />
    public async Task<byte[]> GenerateAsync(ProjectExportModel model)
    {
        var templatePath = Path.Combine(environment.ContentRootPath, "Templates", "presentation-base.pptx");

        if (!File.Exists(templatePath)) throw new EntityNotFoundException($"Шаблон презентации не найден: {templatePath}");

        var pres = new Presentation(templatePath);
        
        try
        {
            // явно захардкодил названия шейпов из зашитой презентации, при использовании другой будет ошибка
            
            // слайд 1
            var slide1 = pres.Slide(1);

            slide1.Shapes.Shape("Text 0").TextBox?.SetText(model.Project.Title);
            slide1.Shapes.Shape("Text 1").TextBox?.SetText($"{model.Project.CityRegion} • {GetYearText(model.Project)}");
            slide1.Shapes.Shape("Text 2").TextBox?.SetText(model.Project.ObjectType);
            slide1.Shapes.Shape("TextBox 6").TextBox?.SetText(model.Project.ShortDescription);
            
            // Добавляем главное изображение
            var mainImage = model.Images.FirstOrDefault(x => x.IsMain);
            
            if (mainImage?.ObjectPath != null)
            {
                var bytes = await minioFileReader.ReadAllBytesAsync(mainImage.ObjectPath);

                using var ms = new MemoryStream(bytes);

                slide1.Shapes.Shape("Image 0").Picture?.Image?.Update(ms);
            }
        
            // слайд 2
            var slide2 = pres.Slide(2);

            slide2.Shapes.Shape("Text 1").TextBox?.SetText(model.Project.LongDescription ?? model.Project.ShortDescription);
            slide2.Shapes.Shape("Text 3").TextBox?.SetText(model.Project.InpadRole);

            // слайд 3
            var slide3 = pres.Slide(3);

            if (model.Metrics is not null)
            {
                var metricsText =
                    $"Общая площадь объекта: {model.Metrics.TotalArea ?? 0}\n" +
                    $"Площадь участка: {model.Metrics.SiteArea ?? 0}\n" +
                    $"Площадь застройки: {model.Metrics.BuildingArea ?? 0}\n" +
                    $"Количество корпусов / секций: {model.Metrics.BuildingsCount ?? 0}\n" +
                    $"Этажность: {model.Metrics.Floors ?? 0}\n" +
                    $"Количество квартир / помещений: {model.Metrics.ApartmentsCount ?? 0}\n" +
                    $"Парковочные места: {model.Metrics.ParkingSpacesCount ?? 0}";

                slide3.Shapes.Shape("Text 1").TextBox?.SetText(metricsText);
            }
        
            // слайд 4
            var slide4 = pres.Slide(4);
            
            var images = model.Images
                .Where(x => x.UseInPresentation)
                .Where(x => !string.IsNullOrWhiteSpace(x.ObjectPath))
                .Take(2)
                .ToList();

            // на всякий, если не найдены изображения для презы, используем главное
            if (images.Count == 0)
            {
                images = model.Images
                    .Where(x => x.IsMain)
                    .Where(x => !string.IsNullOrWhiteSpace(x.ObjectPath))
                    .ToList();
            }
            else if (images.Count < 2)
            {
                var extraImages = model.Images
                    .Where(x => x.IsMain)
                    .Where(x => !string.IsNullOrWhiteSpace(x.ObjectPath))
                    .ToList();
                
                images.AddRange(extraImages);
            }

            var imageShapes = slide4.Shapes
                .Where(s => s.Picture != null
                            && !string.IsNullOrWhiteSpace(s.AltText)
                            && s.AltText.Contains("image"))
                .ToList();

            for (var i = 0; i < images.Count && i < imageShapes.Count; i++)
            {
                var image = images[i];
                var shape = imageShapes[i];

                var bytes = await minioFileReader.ReadAllBytesAsync(image.ObjectPath!);

                using var ms = new MemoryStream(bytes);

                shape.Picture.Image?.Update(ms);
            }
        }
        catch
        {
            throw new PresentationCreationException("Используется неподходящий шаблон презентации");
        }

        using var output = new MemoryStream();
        pres.Save(output);

        return output.ToArray();
    }

    /// <summary>
    /// Возвращает год реализации / проектирования объекта
    /// </summary>
    private static string GetYearText(ProjectCard project)
    {
        return !string.IsNullOrWhiteSpace(project.RealizationYear)
            ? project.RealizationYear
            : project.DesignYearPeriod ?? "Период реализации";
    }
}