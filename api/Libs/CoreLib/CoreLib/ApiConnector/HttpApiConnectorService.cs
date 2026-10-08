using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CoreLib.ApiConnector;

/// <inheritdoc/>
internal sealed class HttpApiConnectorService(
    IHttpClientFactory httpClientFactory,
    IApiConnectorConfigRepository configRepository,
    ILogger<HttpApiConnectorService> logger)
    : IApiConnectorService
{
    public async Task<HttpResponseMessage> SendAsync(string connectorName, object? data = null)
    {
        var configs = await configRepository.GetActiveAsync();
        var config = configs.FirstOrDefault(c => c.Name == connectorName);
        
        if (config is null) throw new InvalidOperationException($"Connector '{connectorName}' not found or inactive");

        var url = ReplacePlaceholders(config.UrlTemplate, data);
        var client = httpClientFactory.CreateClient("ApiConnector");

        using var request = new HttpRequestMessage(new HttpMethod(config.HttpMethod), url);

        var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(config.HeadersJson)
                      ?? new Dictionary<string, string>();
        
        foreach (var header in headers)
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);

        if (config.BodyTemplate is not null && data is not null)
        {
            var body = ReplacePlaceholders(config.BodyTemplate, data);
            request.Content = new StringContent(body, System.Text.Encoding.UTF8, config.ContentType);
        }

        logger.LogInformation("ApiConnector [{Name}] -> {Method} {Url}", connectorName, config.HttpMethod, url);
        return await client.SendAsync(request);
    }

    private static string ReplacePlaceholders(string template, object? data)
    {
        if (data is null) return template;

        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(data, JsonSerializerOptions.Default),
            JsonSerializerOptions.Default);

        if (dict is null) return template;

        var result = template;
        foreach (var kv in dict)
        {
            var value = kv.Value.ValueKind switch
            {
                JsonValueKind.String => kv.Value.GetString() ?? "",
                JsonValueKind.Number => kv.Value.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => "",
                _ => kv.Value.GetRawText()
            };

            result = result.Replace($"{{{kv.Key}}}", value);
        }

        return result;
    }
}
