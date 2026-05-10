using CoreLib.Api.Controllers.ControllerTypes.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CoreLib.Api.Controllers.ControllerTypes.Conventions;

/// <summary>
/// MVC convention, который автоматически добавляет route-prefix
/// в зависимости от базового типа контроллера.
/// </summary>
/// <remarks>
/// Позволяет реализовать логическое наследование маршрутов.
/// Это заменяет необходимость дублировать [Route] в каждом контроллере.
/// </remarks>
public sealed class ControllerRouteConvention : IApplicationModelConvention
{
    private const string ApiPrefix = "api";
    private const string ClientPrefix = "api/client";
    private const string InternalPrefix = "api/internal";
    private const string PublicPrefix = "api/public";
    
    /// <summary>
    /// Применяет route conventions ко всем контроллерам приложения.
    /// Вызывается один раз при построении ApplicationModel (startup phase).
    /// </summary>
    /// <param name="application">Модель приложения MVC.</param>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var controllerType = controller.ControllerType;

            var prefix = GetPrefix(controllerType);

            if (prefix is null)
                continue;

            ApplyPrefix(controller, prefix);
        }
    }

    /// <summary>
    /// Применяет route prefix ко всем селекторам контроллера.
    /// </summary>
    /// <param name="controller">MVC controller model</param>
    /// <param name="prefix">Route prefix</param>
    private static void ApplyPrefix(ControllerModel controller, string prefix)
    {
        foreach (var selector in controller.Selectors)
        {
            selector.AttributeRouteModel = CombineRoute(prefix, selector.AttributeRouteModel);
        }
    }

    /// <summary>
    /// Объединяет route prefix с существующим route-маршрутом.
    /// </summary>
    private static AttributeRouteModel? CombineRoute(string prefix, AttributeRouteModel? existing)
    {
        var prefixRoute = new AttributeRouteModel(new RouteAttribute(prefix));

        return existing is null
            ? prefixRoute
            : AttributeRouteModel.CombineAttributeRouteModel(prefixRoute, existing);
    }

    /// <summary>
    /// Определяет route prefix на основе типа контроллера.
    /// </summary>
    /// <param name="controllerType">Тип контроллера</param>
    /// <returns>Route prefix или null, если не найден</returns>
    private static string? GetPrefix(Type controllerType)
    {
        if (IsDerivedFrom(controllerType, typeof(ClientControllerBase)))
            return ClientPrefix;

        if (IsDerivedFrom(controllerType, typeof(InternalControllerBase)))
            return InternalPrefix;
        
        if (IsDerivedFrom(controllerType, typeof(PublicControllerBase)))
            return PublicPrefix;

        if (IsDerivedFrom(controllerType, typeof(ApiControllerBase)))
            return ApiPrefix;

        return null;
    }

    /// <summary>
    /// Проверяет, наследуется ли тип от указанного базового класса.
    /// </summary>
    /// <param name="type">Проверяемый тип</param>
    /// <param name="baseType">Базовый тип</param>
    private static bool IsDerivedFrom(Type? type, Type baseType)
    {
        while (type is not null && type != typeof(object))
        {
            if (type == baseType)
                return true;

            type = type.BaseType!;
        }

        return false;
    }
}