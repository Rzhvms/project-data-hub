using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.Project;
using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(202606020100)]
public class Date_202606020100_AlterImages_AddIsMainColumn : Migration
{
    private readonly string _imageTbName = EntityMapper.TbName<ProjectImage>();
    
    public override void Up()
    {
        Alter.Table(_imageTbName).AddColumn(EntityMapper.ColName<ProjectImage>(x => x.IsMain))
            .AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        
    }
}