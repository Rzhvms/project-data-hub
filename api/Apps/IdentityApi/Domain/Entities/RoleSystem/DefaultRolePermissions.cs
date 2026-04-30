namespace Domain.Entities.RoleSystem;

/// <summary>
/// Роли, существующие в системе по умолчанию.
/// </summary>
public static class DefaultRolePermissions
{
    /// <summary>
    /// Просмотрщик
    /// </summary>
    public static readonly PermissionList Viewer =
        PermissionList.ViewList |
        PermissionList.ViewCards |
        PermissionList.ExportData;
    
    /// <summary>
    /// Редактор
    /// </summary>
    public static readonly PermissionList Editor =
        Viewer |
        PermissionList.CreateObjects |
        PermissionList.EditCards |
        PermissionList.UploadImages |
        PermissionList.SaveDrafts |
        PermissionList.SubmitForPublish |
        PermissionList.ExportMaterials;

    /// <summary>
    /// Администратор с полным доступом
    /// </summary>
    public static readonly PermissionList Administrator =
        Editor |
        PermissionList.Publish |
        PermissionList.DeleteObjects |
        PermissionList.ConfigureTemplates |
        PermissionList.ManageUsers |
        PermissionList.ManageDirectories;
}