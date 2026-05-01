using CoreLib.Api.Controllers.ControllerTypes;
using IdentityLib.Jwt.RsaKeys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers.System;

/// <summary>
/// Контроллер предоставляет публичные метаданные Identity сервиса
/// в соответствии со стандартами OpenID Connect и JWT (JWKS).
/// </summary>
/// <remarks>
/// Используется для:
/// - валидации JWT токенов внешними системами (Gateway, клиенты)
/// - автоматического discovery конфигурации Identity сервиса
/// 
/// Эндпоинты соответствуют спецификации:
/// - RFC 7517 (JSON Web Key Set - JWKS)
/// - OpenID Connect Discovery
/// - RFC 8615 (.well-known URIs)
/// </remarks>
[Route(".well-known")]
public class WellKnownController(IRsaKeyProvider keyProvider, IConfiguration configuration) : SystemControllerBase
{
    /// <summary>
    /// Возвращает JSON Web Key Set — публичные ключи для проверки JWT.
    /// </summary>
    /// <remarks>
    /// Используется внешними сервисами (API Gateway) для валидации подписи JWT токенов без доступа к приватному ключу.
    /// </remarks>
    /// <returns>Набор RSA публичных ключей в формате JWKS</returns>
    [HttpGet("jwks.json")]
    public IActionResult GetJwks()
    {
        var rsa = keyProvider.ValidationKey.Rsa!;
        var parameters = rsa.ExportParameters(false);

        return Ok(new
        {
            keys = new[]
            {
                new
                {
                    kty = "RSA",
                    use = "sig",
                    alg = "RS256",
                    n = Base64UrlEncoder.Encode(parameters.Modulus),
                    e = Base64UrlEncoder.Encode(parameters.Exponent),
                    kid = "main-key"
                }
            }
        });
    }

    /// <summary>
    /// OpenID Connect discovery endpoint.
    /// </summary>
    /// <remarks>
    /// Позволяет клиентам автоматически определить:
    /// - issuer сервиса
    /// - JWKS endpoint
    /// - будущие OAuth/OIDC endpoints
    /// </remarks>
    /// <returns>OpenID configuration документ</returns>
    [HttpGet("openid-configuration")]
    public IActionResult GetConfig()
    {
        var issuer = configuration["Jwt:AccessTokenSettings:Issuer"];
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

        return Ok(new
        {
            issuer = issuer,
            jwks_uri = $"{baseUrl}/.well-known/jwks.json"
        });
    }
}