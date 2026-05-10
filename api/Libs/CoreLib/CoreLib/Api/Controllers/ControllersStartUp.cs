using CoreLib.Api.Controllers.ControllerTypes.Conventions;
using CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;
using CoreLib.Api.Controllers.ControllerTypes.Policies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.Api.Controllers;

public static class ControllersStartUp
{
    /// <summary>
    /// Добавление зависимостей
    /// </summary>
    public static void AddCoreControllers(this IServiceCollection services)
    {
        services.AddScoped<PublicRequestFilter>();
        services.AddScoped<ClientRequestFilter>();
        services.AddScoped<InternalRequestFilter>();
        
        services.Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new ControllerRouteConvention());
        });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ControllerPolicies.Client, policy =>
            {
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy(ControllerPolicies.Internal, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", ControllerPolicies.Internal);
            });
        });
    }
}