using System.Reflection;
using CoreLib.Api.Controllers.ControllerTypes;
using CoreLib.Api.Controllers.ControllerTypes.Base;
using CoreLib.Api.Controllers.ControllerTypes.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Xunit;

namespace CoreLib.Tests.Api.Controllers.ControllerTypes.Conventions;

/// <summary>
/// Тесты для <see cref="ControllerRouteConvention"/>.
/// Проверяют, что convention корректно назначает route-prefix в зависимости от базового типа контроллера,
/// а также не изменяет контроллеры, которые не наследуются от поддерживаемых базовых классов.
/// </summary>
public sealed class ControllerRouteConventionTests
{
    private readonly ControllerRouteConvention _convention = new();

    /// <summary>
    /// Проверяет, что для контроллера, наследующегося от <see cref="ApiControllerBase"/>,
    /// префикс маршрута становится <c>api</c>.
    /// </summary>
    [Fact]
    public void Apply_ShouldAddApiPrefix_ForApiControllerBase()
    {
        var application = CreateApplicationModel(typeof(TestApiController), "users");

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("api/users", route);
    }

    /// <summary>
    /// Проверяет, что для контроллера, наследующегося от <see cref="ClientControllerBase"/>,
    /// префикс маршрута становится <c>api/client</c>.
    /// </summary>
    [Fact]
    public void Apply_ShouldAddClientPrefix_ForClientControllerBase()
    {
        var application = CreateApplicationModel(typeof(TestClientController), "profiles");

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("api/client/profiles", route);
    }

    /// <summary>
    /// Проверяет, что для контроллера, наследующегося от <see cref="InternalControllerBase"/>,
    /// префикс маршрута становится <c>api/internal</c>.
    /// </summary>
    [Fact]
    public void Apply_ShouldAddInternalPrefix_ForInternalControllerBase()
    {
        var application = CreateApplicationModel(typeof(TestInternalController), "audit");

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("api/internal/audit", route);
    }

    /// <summary>
    /// Проверяет, что для контроллера, наследующегося от <see cref="PublicControllerBase"/>,
    /// префикс маршрута становится <c>api/public</c>.
    /// </summary>
    [Fact]
    public void Apply_ShouldAddPublicPrefix_ForPublicControllerBase()
    {
        var application = CreateApplicationModel(typeof(TestPublicController), "info");

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("api/public/info", route);
    }

    /// <summary>
    /// Проверяет, что если контроллер не наследуется от поддерживаемых базовых типов,
    /// convention не изменяет его маршрут.
    /// </summary>
    [Fact]
    public void Apply_ShouldNotChangeRoute_ForUnrelatedController()
    {
        var application = CreateApplicationModel(typeof(UnrelatedController), "plain-route");

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("plain-route", route);
    }

    /// <summary>
    /// Проверяет, что если у контроллера нет существующего route, convention создает его только на основе prefix.
    /// </summary>
    [Fact]
    public void Apply_ShouldCreatePrefixOnlyRoute_WhenSelectorHasNoExistingRoute()
    {
        var application = CreateApplicationModel(typeof(TestApiController), null);

        _convention.Apply(application);

        var route = GetRouteTemplate(application.Controllers[0]);

        Assert.Equal("api", route);
    }

    /// <summary>
    /// Проверяет, что все селекторы одного контроллера получают одинаковый prefix.
    /// </summary>
    [Fact]
    public void Apply_ShouldApplyPrefixToAllSelectors()
    {
        var controllerModel = CreateControllerModel(typeof(TestClientController), "first", "second");
        var application = new ApplicationModel
        {
            Controllers = { controllerModel }
        };

        _convention.Apply(application);

        Assert.Equal("api/client/first", GetRouteTemplate(controllerModel, 0));
        Assert.Equal("api/client/second", GetRouteTemplate(controllerModel, 1));
    }

    /// <summary>
    /// Проверяет, что при нескольких контроллерах convention корректно обрабатывает каждый из них независимо.
    /// </summary>
    [Fact]
    public void Apply_ShouldProcessAllControllersIndependently()
    {
        var application = new ApplicationModel
        {
            Controllers =
            {
                CreateControllerModel(typeof(TestApiController), "users"),
                CreateControllerModel(typeof(TestPublicController), "docs"),
                CreateControllerModel(typeof(UnrelatedController), "plain")
            }
        };

        _convention.Apply(application);

        Assert.Equal("api/users", GetRouteTemplate(application.Controllers[0]));
        Assert.Equal("api/public/docs", GetRouteTemplate(application.Controllers[1]));
        Assert.Equal("plain", GetRouteTemplate(application.Controllers[2]));
    }

    /// <summary>
    /// Создает <see cref="ApplicationModel"/> с одним контроллером и одним селектором.
    /// </summary>
    /// <param name="controllerType">Тип тестового контроллера.</param>
    /// <param name="routeTemplate">Исходный шаблон маршрута. Если <c>null</c>, selector создается без маршрута.</param>
    /// <returns>Готовая модель приложения MVC.</returns>
    private static ApplicationModel CreateApplicationModel(Type controllerType, string? routeTemplate)
    {
        return new ApplicationModel
        {
            Controllers =
            {
                CreateControllerModel(controllerType, routeTemplate)
            }
        };
    }

    /// <summary>
    /// Создает <see cref="ControllerModel"/> для указанного типа контроллера.
    /// </summary>
    /// <param name="controllerType">Тип контроллера.</param>
    /// <param name="routeTemplate">Шаблон маршрута для селектора.</param>
    /// <returns>Модель контроллера MVC.</returns>
    private static ControllerModel CreateControllerModel(Type controllerType, string? routeTemplate)
    {
        var controllerModel = new ControllerModel(controllerType.GetTypeInfo(), Array.Empty<object>());

        var selector = new SelectorModel();
        if (routeTemplate is not null)
        {
            selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(routeTemplate));
        }

        controllerModel.Selectors.Add(selector);
        return controllerModel;
    }

    /// <summary>
    /// Создает <see cref="ControllerModel"/> с несколькими селекторами.
    /// </summary>
    /// <param name="controllerType">Тип контроллера.</param>
    /// <param name="routes">Шаблоны маршрутов для каждого селектора.</param>
    /// <returns>Модель контроллера MVC.</returns>
    private static ControllerModel CreateControllerModel(Type controllerType, params string[] routes)
    {
        var controllerModel = new ControllerModel(controllerType.GetTypeInfo(), Array.Empty<object>());

        foreach (var route in routes)
        {
            controllerModel.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(route))
            });
        }

        return controllerModel;
    }

    /// <summary>
    /// Возвращает итоговый шаблон маршрута первого селектора контроллера.
    /// </summary>
    /// <param name="controllerModel">MVC-модель контроллера.</param>
    /// <returns>Итоговый шаблон маршрута.</returns>
    private static string? GetRouteTemplate(ControllerModel controllerModel)
        => GetRouteTemplate(controllerModel, 0);

    /// <summary>
    /// Возвращает итоговый шаблон маршрута селектора по индексу.
    /// </summary>
    /// <param name="controllerModel">MVC-модель контроллера.</param>
    /// <param name="selectorIndex">Индекс селектора.</param>
    /// <returns>Итоговый шаблон маршрута.</returns>
    private static string? GetRouteTemplate(ControllerModel controllerModel, int selectorIndex)
    {
        var route = controllerModel.Selectors[selectorIndex].AttributeRouteModel;
        return route?.Template;
    }

    /// <summary>
    /// Тестовый контроллер, наследующийся от <see cref="ApiControllerBase"/>.
    /// </summary>
    private sealed class TestApiController : ApiControllerBase;

    /// <summary>
    /// Тестовый контроллер, наследующийся от <see cref="ClientControllerBase"/>.
    /// </summary>
    private sealed class TestClientController : ClientControllerBase;

    /// <summary>
    /// Тестовый контроллер, наследующийся от <see cref="InternalControllerBase"/>.
    /// </summary>
    private sealed class TestInternalController : InternalControllerBase;

    /// <summary>
    /// Тестовый контроллер, наследующийся от <see cref="PublicControllerBase"/>.
    /// </summary>
    private sealed class TestPublicController : PublicControllerBase;

    /// <summary>
    /// Контроллер без наследования от поддерживаемых базовых типов.
    /// </summary>
    private sealed class UnrelatedController;
}