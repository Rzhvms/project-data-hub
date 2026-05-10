using CoreLib.Database.DapperExtensions.EntityMapper;
using Domain.Entities.Project;
using Domain.Entities.Project.Categories;
using Domain.Entities.Project.Roles;
using FluentMigrator;

namespace Infrastructure.Migrations;

/// <summary>
/// Миграция по созданию таблиц, связанных с проектами
/// </summary>
[Migration(202605012350)]
public class Date_202605012350_AddProjectTables : Migration
{
    private readonly string _projectCardTb = EntityMapper.TbName<ProjectCard>();
    private readonly string _projectCardDraftTb = EntityMapper.TbName<ProjectCardDraft>();
    private readonly string _projectMetricsTb = EntityMapper.TbName<ProjectMetrics>();
    private readonly string _projectCategoryTb = EntityMapper.TbName<ProjectCategory>();
    private readonly string _projectCategoryLinkTb = EntityMapper.TbName<ProjectCategoryLink>();
    private readonly string _projectParticipantTb = EntityMapper.TbName<ProjectParticipant>();
    private readonly string _projectParticipantLinkTb = EntityMapper.TbName<ProjectParticipantLink>();

    public override void Up()
    {
        // Создание таблицы ProjectCard
        Create.Table(_projectCardTb)
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.Title)).AsString().NotNullable()
                .WithColumnDescription("Полное публичное название объекта")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.ShortTitle)).AsString().Nullable()
                .WithColumnDescription("Сокращенное название для списков и презентаций")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.CityRegion)).AsString().NotNullable()
                .WithColumnDescription("Место расположения объекта")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.Address)).AsString().Nullable()
                .WithColumnDescription("Точный или ориентировочный адрес")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.DesignYearPeriod)).AsString().Nullable()
                .WithColumnDescription("Год или период проектирования")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.RealizationYear)).AsInt32().Nullable()
                .WithColumnDescription("Год реализации")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.ProjectStatus)).AsString(255).NotNullable()
                .WithColumnDescription("Статус проекта")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.ObjectType)).AsString(255).NotNullable()
                .WithColumnDescription("Тип объекта")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.Customer)).AsString(255).Nullable()
                .WithColumnDescription("Наименование заказчика")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.InpadRole)).AsString(255).NotNullable()
                .WithColumnDescription("Роль компании ИНПАД")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.DesignStage)).AsString(255).Nullable()
                .WithColumnDescription("Статус проектирования")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.ShortDescription)).AsString(255).NotNullable()
                .WithColumnDescription("Короткий текст для карточек, анонсов и презентаций")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.LongDescription)).AsString().Nullable()
                .WithColumnDescription("Полное описание объекта")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.PublicationStatus)).AsInt16().NotNullable().WithDefaultValue(1)
                .WithColumnDescription("1 - Черновик. 2 - На проверке. 3 - Требуется доработка. 4 - Опубликован. 5 - Архивный. 6 - Ошибка публикации.")
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.UpdatedAt)).AsDateTime().Nullable()
            .WithColumn(EntityMapper.ColName<ProjectCard>(x => x.Publisher)).AsString(255).Nullable();
        
        // Создание таблицы ProjectCardDraft
        Create.Table(_projectCardDraftTb)
            .WithColumn(EntityMapper.ColName<ProjectCardDraft>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectData)).AsCustom("jsonb").NotNullable();
        
        // Создание внешнего ключа с ProjectCardDraft.ProjectId на ProjectCard.Id
        Create.ForeignKey("fk_projectCard_id_draft")
            .FromTable(_projectCardDraftTb).ForeignColumn(EntityMapper.ColName<ProjectCardDraft>(x => x.ProjectId))
            .ToTable(_projectCardTb).PrimaryColumn(EntityMapper.ColName<ProjectCard>(x => x.Id));
           
        // Создание таблицы ProjectMetrics
        Create.Table(_projectMetricsTb)
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.TotalArea)).AsDecimal().Nullable()
                .WithColumnDescription("Общая площадь объекта")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.SiteArea)).AsDecimal().Nullable()
                .WithColumnDescription("Площадь участка")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.BuildingArea)).AsDecimal().Nullable()
                .WithColumnDescription("Площадь застройки")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.BuildingsCount)).AsInt32().Nullable()
                .WithColumnDescription("Количество корпусов или секций")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.Floors)).AsInt32().Nullable()
                .WithColumnDescription("Этажность объекта")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.ApartmentsCount)).AsInt32().Nullable()
                .WithColumnDescription("Количество квартир или помещений")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.ParkingSpacesCount)).AsInt32().Nullable()
                .WithColumnDescription("Количество парковочных мест")
            .WithColumn(EntityMapper.ColName<ProjectMetrics>(x => x.JsonData)).AsCustom("jsonb").Nullable()
                .WithColumnDescription("Дополнительные данные для расширения модели");
            
        // Создание внешнего ключа с ProjectMetrics.ProjectId на ProjectCard.Id
        Create.ForeignKey("fk_projectCard_id_metrics")
            .FromTable(_projectMetricsTb).ForeignColumn(EntityMapper.ColName<ProjectMetrics>(x => x.ProjectId))
            .ToTable(_projectCardTb).PrimaryColumn(EntityMapper.ColName<ProjectCard>(x => x.Id));
        
        // Создание таблицы ProjectCategory
        Create.Table(_projectCategoryTb)
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.Name)).AsString(255).NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.Description)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.SortOrder)).AsInt32().Nullable()
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.IsSystem)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategory>(x => x.UpdatedAt)).AsDateTime().Nullable();

        // Создание таблицы для связи M:N между ProjectCategory и ProjectCard
        Create.Table(_projectCategoryLinkTb)
            .WithColumn(EntityMapper.ColName<ProjectCategoryLink>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategoryLink>(x => x.ProjectId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectCategoryLink>(x => x.CategoryId)).AsGuid().NotNullable();
        
        // Создание внешнего ключа с ProjectCategoryLink.ProjectId на ProjectCard.Id
        Create.ForeignKey("fk_projectCardCategory_ProjectId")
            .FromTable(_projectCategoryLinkTb).ForeignColumn(EntityMapper.ColName<ProjectCategoryLink>(x => x.ProjectId))
            .ToTable(_projectCardTb).PrimaryColumn(EntityMapper.ColName<ProjectCard>(x => x.Id));
        
        // Создание внешнего ключа с ProjectCategoryLink.ProjectId на ProjectCategory.Id
        Create.ForeignKey("fk_projectCardCategory_CategoryId")
            .FromTable(_projectCategoryLinkTb).ForeignColumn(EntityMapper.ColName<ProjectCategoryLink>(x => x.CategoryId))
            .ToTable(_projectCategoryTb).PrimaryColumn(EntityMapper.ColName<ProjectCategory>(x => x.Id));
        
        // Создание таблицы ProjectParticipant
        Create.Table(_projectParticipantTb)
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.Name)).AsString(255).NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.Description)).AsString().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.SortOrder)).AsInt32().Nullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.IsActive)).AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.IsSystem)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.CreatedAt)).AsDateTime().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipant>(x => x.UpdatedAt)).AsDateTime().Nullable();
        
        // Создание таблицы для связи M:N между ProjectParticipant и ProjectCard
        Create.Table(_projectParticipantLinkTb)
            .WithColumn(EntityMapper.ColName<ProjectParticipantLink>(x => x.Id)).AsGuid().PrimaryKey().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipantLink>(x => x.ProjectId)).AsGuid().NotNullable()
            .WithColumn(EntityMapper.ColName<ProjectParticipantLink>(x => x.ParticipantId)).AsGuid().NotNullable();
        
        // Создание внешнего ключа с ProjectParticipantLink.ProjectId на ProjectCard.Id
        Create.ForeignKey("fk_projectCardParticipant_ProjectId")
            .FromTable(_projectParticipantLinkTb).ForeignColumn(EntityMapper.ColName<ProjectParticipantLink>(x => x.ProjectId))
            .ToTable(_projectCardTb).PrimaryColumn(EntityMapper.ColName<ProjectCard>(x => x.Id));
        
        // Создание внешнего ключа с ProjectParticipantLink.ParticipantId на ProjectParticipant.Id
        Create.ForeignKey("fk_projectCardParticipant_ParticipantId")
            .FromTable(_projectParticipantLinkTb).ForeignColumn(EntityMapper.ColName<ProjectParticipantLink>(x => x.ParticipantId))
            .ToTable(_projectParticipantTb).PrimaryColumn(EntityMapper.ColName<ProjectParticipant>(x => x.Id));
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_projectCardParticipant_ParticipantId").OnTable(_projectParticipantLinkTb);
        Delete.ForeignKey("fk_projectCardParticipant_ProjectId").OnTable(_projectParticipantLinkTb);

        Delete.Table(_projectParticipantLinkTb);
        Delete.Table(_projectParticipantTb);
        
        Delete.ForeignKey("fk_projectCardCategory_CategoryId").OnTable(_projectCategoryLinkTb);
        Delete.ForeignKey("fk_projectCardCategory_ProjectId").OnTable(_projectCategoryLinkTb);
         
        Delete.Table(_projectCategoryLinkTb);
        Delete.Table(_projectCategoryTb);
        
        Delete.ForeignKey("fk_projectCard_id_metrics").OnTable(_projectMetricsTb);
        Delete.Table(_projectMetricsTb);
        
        Delete.ForeignKey("fk_projectCard_id_draft").OnTable(_projectCardDraftTb);
        Delete.Table(_projectCardDraftTb);
        Delete.Table(_projectCardTb);
    }
}