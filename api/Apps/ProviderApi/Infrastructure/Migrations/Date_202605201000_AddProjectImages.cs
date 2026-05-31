using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.Project;
using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(202605201000)]
public class Date_202605201000_AddProjectImages : Migration
{
    private readonly string _imageTbName = EntityMapper.TbName<ProjectImage>();
    
    public override void Up()
    {
        Create.Table(_imageTbName)
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.Id)).AsGuid().PrimaryKey()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.ProjectId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.ObjectPath)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.Title)).AsString(255).Nullable()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.Description)).AsString().Nullable()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.AlternativeText)).AsString().Nullable()
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.UseInSite)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.UseInPresentation)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<ProjectImage>(x => x.UseInPortfolio)).AsBoolean().NotNullable().WithDefaultValue(true);
            
        Create.ForeignKey($"fk_{_imageTbName}_project")
            .FromTable(_imageTbName).ForeignColumn(EntityMapper.ColName<ProjectImage>(x => x.ProjectId))
            .ToTable(EntityMapper.TbName<ProjectCard>()).PrimaryColumn(EntityMapper.ColName<ProjectCard>(x => x.Id))
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Index($"ix_{_imageTbName}_projectid")
            .OnTable(_imageTbName)
            .OnColumn(EntityMapper.ColName<ProjectImage>(x => x.ProjectId))
            .Ascending();
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}