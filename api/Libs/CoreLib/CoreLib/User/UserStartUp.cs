using Microsoft.Extensions.DependencyInjection;

namespace CoreLib.User;

public static class UserStartUp
{
    public static void AddCoreUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
