namespace Domain.Entities.RoleSystem;

/// <summary>
/// Список прав
/// </summary>
///[Flags]
[Flags]
public enum PermissionList
{
    None = 0,

    #region Просмотр

    /// <summary>
    /// Просмотр списка объектов
    /// </summary>
    ViewList = 1 << 0,
    
    /// <summary>
    /// Просмотр карточек объектов
    /// </summary>
    ViewCards = 1 << 1,
    
    /// <summary>
    /// Выгрузка данных
    /// </summary>
    ExportData = 1 << 2,

    #endregion

    #region Редактирование
    
    /// <summary>
    /// Создание объектов
    /// </summary>
    CreateObjects = 1 << 3,
    
    /// <summary>
    /// Редактирование карточек
    /// </summary>
    EditCards = 1 << 4,
    
    /// <summary>
    /// Загрузка изображений
    /// </summary>
    UploadImages = 1 << 5,
    
    /// <summary>
    /// Сохранение черновиков
    /// </summary>
    SaveDrafts = 1 << 6,
    
    /// <summary>
    /// Отправка на публикацию
    /// </summary>
    SubmitForPublish = 1 << 7,
    
    /// <summary>
    /// Выгрузка материалов
    /// </summary>
    ExportMaterials = 1 << 8,

    #endregion
    
    #region Публикация и управление
    
    /// <summary>
    /// Публикация
    /// </summary>
    Publish = 1 << 9,
    
    /// <summary>
    /// Удаление объектов
    /// </summary>
    DeleteObjects = 1 << 10,
    
    /// <summary>
    /// Настройка шаблонов
    /// </summary>
    ConfigureTemplates = 1 << 11,
    
    /// <summary>
    /// Управление пользователями
    /// </summary>
    ManageUsers = 1 << 12,
    
    /// <summary>
    /// Управление справочниками
    /// </summary>
    ManageDirectories = 1 << 13,

    #endregion
}

