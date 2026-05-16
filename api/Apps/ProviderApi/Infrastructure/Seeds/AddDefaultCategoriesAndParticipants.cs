using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.Project.Categories;
using Domain.Entities.Project.Categories.Constants;
using Domain.Entities.Project.Roles;
using Domain.Entities.Project.Roles.Constants;
using FluentMigrator;

namespace Infrastructure.Seeds;

/// <summary>
/// Добавление системных категорий
/// </summary>
[Migration(202605021430)]
public class AddDefaultCategoriesAndParticipants : Migration
{
    private readonly string _categoryTableName = EntityMapper.TbName<ProjectCategory>();
    private readonly string _participantTableName  = EntityMapper.TbName<ProjectParticipant>();
    
    public override void Up()
    {
        var createdAt = DateTime.UtcNow;

        var categories = new[]
        {
            DefaultCategoryNames.ResidentialComplexes,
            DefaultCategoryNames.PublicBuildings,
            DefaultCategoryNames.CommercialRealEstate,
            DefaultCategoryNames.Improvement,
            DefaultCategoryNames.UrbanPlanning,
            DefaultCategoryNames.Interior,
            DefaultCategoryNames.Concept,
            DefaultCategoryNames.ProjectDocumentation,
            DefaultCategoryNames.WorkingDocumentation,
            DefaultCategoryNames.DigitalTechnologies
        };

        foreach (var categoryName in categories)
        {
            Insert.IntoTable(_categoryTableName).Row(new
            {
                Id = Guid.NewGuid(),
                Name = categoryName,
                Description = string.Empty,
                IsActive = true,
                IsSystem = true,
                CreatedAt = createdAt,
                UpdatedAt = (DateTime?)null
            });
        }

        var paricipants = new[]
        {
            DefaultParticipantNames.ProjectManager,
            DefaultParticipantNames.ChiefArchitect,
            DefaultParticipantNames.ChiefEngineer,
            DefaultParticipantNames.Engineer,
            DefaultParticipantNames.Architect,
            DefaultParticipantNames.BimSpecialist,
            DefaultParticipantNames.Visualizer,
            DefaultParticipantNames.PartnerContractor
        };
        
        foreach (var paricipant in paricipants)
        {
            Insert.IntoTable(_participantTableName).Row(new
            {
                Id = Guid.NewGuid(),
                Name = paricipant,
                Description = string.Empty,
                IsActive = true,
                IsSystem = true,
                CreatedAt = createdAt,
                UpdatedAt = (DateTime?)null
            });
        }
    }

    public override void Down()
    {
        //throw new NotImplementedException();
    }
}