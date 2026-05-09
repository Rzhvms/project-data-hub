using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityLib.Exceptions;
using IdentityLib.Jwt.Entities;
using IdentityLib.Jwt.Interfaces;
using IdentityLib.Jwt.RsaKeys.Interfaces;
using IdentityLib.Jwt.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityLib.Jwt.Services;

/// <summary>
/// Сервис генерации и проверки JWT.
/// </summary>
public sealed class JwtGenerationService : IJwtGenerationService
{
    /// <summary>
    /// Время жизни access-токена для сценария сброса пароля.
    /// </summary>
    private static readonly TimeSpan PasswordResetAccessTokenLifetime = TimeSpan.FromMinutes(5);
    private const string JwtAuthenticationType = "Bearer";

    private readonly JwtSettings _settings;
    private readonly RsaSecurityKey _signingKey;
    private readonly RsaSecurityKey _validationKey;
    private readonly TimeProvider _timeProvider;

    /// 
    public JwtGenerationService(IOptions<JwtSettings> settings, IRsaKeyProvider keyProvider, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(keyProvider);
        ArgumentNullException.ThrowIfNull(timeProvider);

        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _signingKey = keyProvider.SigningKey;
        _validationKey = keyProvider.ValidationKey;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public string GenerateAccessToken(JwtUserData user, bool isRestoringPassword = false)
    {
        ArgumentNullException.ThrowIfNull(user);

        var now = _timeProvider.GetUtcNow();
        var expires = isRestoringPassword
            ? now.Add(PasswordResetAccessTokenLifetime)
            : now.AddSeconds(_settings.AccessTokenSettings.LifeTimeInSeconds);

        var claims = BuildIdTokenClaims(user);

        return CreateToken(
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingKey: _signingKey);
    }

    /// <inheritdoc />
    public string GenerateIdToken(JwtUserData user)
    {
        ArgumentNullException.ThrowIfNull(user);
   
        var now = _timeProvider.GetUtcNow();
        var expires = now.AddSeconds(_settings.AccessTokenSettings.LifeTimeInSeconds);

        var claims = BuildIdTokenClaims(user);

        return CreateToken(
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingKey: _signingKey);
    }

    /// <inheritdoc />
    public string GenerateRefreshToken()
    {
        var buffer = new byte[_settings.RefreshTokenSettings.Length];
        RandomNumberGenerator.Fill(buffer);
        return Convert.ToBase64String(buffer);
    }

    /// <inheritdoc />
    public int GetRefreshTokenLifetimeInMinutes()
        => _settings.RefreshTokenSettings.LifeTimeInMinutes;

    /// <inheritdoc />
    public Guid GetUserIdFromToken(string token)
    {
        var principal = ValidateToken(token, validateLifetime: false);

        var rawUserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(rawUserId) || !Guid.TryParse(rawUserId, out var userId))
        {
            throw new InvalidTokenException("Token does not contain a valid UserId.");
        }

        return userId;
    }

    /// <inheritdoc />
    public bool IsTokenValid(string token, bool validateLifeTime)
    {
        try
        {
            _ = ValidateToken(token, validateLifetime: validateLifeTime);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Построение клэймов для access-токена.
    /// </summary>
    private static IEnumerable<Claim> BuildAccessTokenClaims(JwtUserData user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString("D")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };

        if (user.Claims is not null)
        {
            claims.AddRange(user.Claims);
        }

        return claims;
    }

    /// <summary>
    /// Построение клэймов для id-токена.
    /// </summary>
    private static IEnumerable<Claim> BuildIdTokenClaims(JwtUserData user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString("D")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }
        
        if (!string.IsNullOrWhiteSpace(user.FirstName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(user.LastName))
        {
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
        }
        
        if (!string.IsNullOrWhiteSpace(user.Patronymic))
        {
            claims.Add(new Claim("patronymic", user.Patronymic));
        }

        if (user.Claims is not null)
        {
            claims.AddRange(user.Claims);
        }

        return claims;
    }

    /// <summary>
    /// Формирование токена.
    /// </summary>
    private string CreateToken(string issuer, string audience, IEnumerable<Claim> claims, 
        DateTime notBefore, DateTime expires, RsaSecurityKey signingKey)
    {
        var identity = new ClaimsIdentity(claims, JwtAuthenticationType);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateJwtSecurityToken(
            issuer: issuer,
            audience: audience,
            subject: identity,
            notBefore: notBefore,
            expires: expires,
            issuedAt: notBefore,
            signingCredentials: credentials);

        return handler.WriteToken(token);
    }

    /// <summary>
    /// Валидация токена.
    /// </summary>
    private ClaimsPrincipal ValidateToken(string token, bool validateLifetime)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidTokenException("Token is empty.");
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            RequireSignedTokens = true,
            RequireExpirationTime = validateLifetime,
            ValidIssuer = _settings.AccessTokenSettings.Issuer,
            ValidAudience = _settings.AccessTokenSettings.Audience,
            IssuerSigningKey = _validationKey,
            ClockSkew = TimeSpan.Zero
        };

        var handler = new JwtSecurityTokenHandler();

        try
        {
            return handler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception ex) when (ex is SecurityTokenException or ArgumentException or FormatException)
        {
            throw new InvalidTokenException("Token is invalid.", ex);
        }
    }
}