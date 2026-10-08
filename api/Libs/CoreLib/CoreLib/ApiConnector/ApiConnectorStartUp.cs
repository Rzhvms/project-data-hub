using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreLib.ApiConnector;

/// <summary>
/// Регистрация Api-коннекторов
/// </summary>
public static class ApiConnectorStartUp
{
    public static void AddApiConnectors(this IServiceCollection services, IConfiguration configuration)
    {
        var enabled = configuration.GetValue<bool>("ApiConnectors__Enabled");

        if (enabled)
        {
            services.AddHttpClient("ApiConnector");
            services.AddSingleton<IApiConnectorService, HttpApiConnectorService>();
        }
        else
        {
            services.AddSingleton<IApiConnectorService, NullApiConnectorService>();
        }
    }
}

/// <summary>
/// Заглушка, когда коннекторы отключены
/// </summary>
internal sealed class NullApiConnectorService(ILogger<NullApiConnectorService> logger) : IApiConnectorService
{
    public Task<HttpResponseMessage> SendAsync(string connectorName, object? data = null)
    {
        logger.LogDebug("ApiConnector {Name} skipped (connectors disabled)", connectorName);
        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NoContent));
    }
}
