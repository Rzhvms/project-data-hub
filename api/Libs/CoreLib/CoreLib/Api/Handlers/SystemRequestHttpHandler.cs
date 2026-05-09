namespace CoreLib.Api.Handlers;

/// <summary>
/// Хэндлер для проксирования системных запровсов.
/// </summary>
public class SystemRequestHttpHandler : HttpClientHandler
{
    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-System-Request", "proxy");
        return await base.SendAsync(request, cancellationToken);
    }
}