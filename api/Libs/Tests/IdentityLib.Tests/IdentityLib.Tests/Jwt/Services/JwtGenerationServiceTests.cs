using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityLib.Exceptions;
using IdentityLib.Jwt.Entities;
using IdentityLib.Jwt.Interfaces;
using IdentityLib.Jwt.RsaKeys.Interfaces;
using IdentityLib.Jwt.Services;
using IdentityLib.Jwt.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace IdentityLib.Tests.Jwt.Services;

/// <summary>
/// Unit-тесты для <see cref="JwtGenerationService"/>.
/// Покрывают генерацию access token, id token, refresh token, валидацию токенов,
/// извлечение user id и негативные сценарии.
/// </summary>
public sealed class JwtGenerationServiceTests : IDisposable
{
    private readonly JwtSettings _settings;
    private readonly TestRsaKeyProvider _keyProvider;
    private readonly IJwtGenerationService _service;
    private readonly JwtUserData _user;
    private readonly DateTimeOffset _now = DateTimeOffset.UtcNow.AddMinutes(-1);

    /// <summary>
    /// Создает тестовый экземпляр сервиса с фиксированными настройками, временем и RSA-ключами.
    /// </summary>
    public JwtGenerationServiceTests()
    {
        _settings = CreateSettings();
        _keyProvider = new TestRsaKeyProvider();

        var timeProvider = new FixedTimeProvider(_now);
        _service = new JwtGenerationService(Options.Create(_settings), _keyProvider, timeProvider);

        _user = new JwtUserData
        {
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            UserName = "user",
            Email = "user@example.com",
            FirstName = "name",
            LastName = "surname",
            Claims =
            [
                new Claim("scope", "profile"),
                new Claim("tenant", "identity")
            ]
        };
    }

    /// <summary>
    /// Освобождает криптографические ресурсы, созданные для тестов.
    /// </summary>
    public void Dispose()
    {
        _keyProvider.Dispose();
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает <see cref="ArgumentNullException"/>,
    /// если не передан объект настроек.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenSettingsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new JwtGenerationService(null!, _keyProvider, new FixedTimeProvider(_now)));
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает <see cref="ArgumentNullException"/>,
    /// если не передан провайдер ключей.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenKeyProviderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new JwtGenerationService(Options.Create(_settings), null!, new FixedTimeProvider(_now)));
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает <see cref="ArgumentNullException"/>,
    /// если не передан provider времени.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenTimeProviderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new JwtGenerationService(Options.Create(_settings), _keyProvider, null!));
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает <see cref="ArgumentNullException"/>,
    /// если <see cref="IOptions{TOptions}.Value"/> равен null.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenSettingsValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new JwtGenerationService(Options.Create<JwtSettings>(null!), _keyProvider, new FixedTimeProvider(_now)));
    }

    /// <summary>
    /// Проверяет, что access token генерируется в корректном формате JWT.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ShouldReturnValidJwtString()
    {
        var token = _service.GenerateAccessToken(_user);

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Contains('.', token);
    }

    /// <summary>
    /// Проверяет, что access token содержит ожидаемые claims пользователя и дополнительные claims.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ShouldContainUserIdAndCustomClaims()
    {
        var token = _service.GenerateAccessToken(_user);
        var jwt = ReadToken(token);

        Assert.Equal(_settings.AccessTokenSettings.Issuer, jwt.Issuer);
        Assert.Contains(_settings.AccessTokenSettings.Audience, jwt.Audiences);

        Assert.Contains(jwt.Claims, c => c.Type == "nameid" && c.Value == _user.UserId.ToString("D"));
        Assert.Contains(jwt.Claims, c => c.Type == "scope" && c.Value == "profile");
        Assert.Contains(jwt.Claims, c => c.Type == "tenant" && c.Value == "identity");
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
    }

    /// <summary>
    /// Проверяет, что access token использует корректный срок жизни из настроек.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ShouldUseConfiguredLifetime()
    {
        var token = _service.GenerateAccessToken(_user);
        var jwt = ReadToken(token);

        Assert.Equal(TruncateToSeconds(_now.UtcDateTime), jwt.ValidFrom);
        Assert.Equal(TruncateToSeconds(_now.AddSeconds(_settings.AccessTokenSettings.LifeTimeInSeconds).UtcDateTime), jwt.ValidTo);
    }

    /// <summary>
    /// Проверяет, что access token для сценария восстановления пароля получает фиксированное время жизни 5 минут.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ForPasswordReset_ShouldUseFiveMinuteLifetime()
    {
        var token = _service.GenerateAccessToken(_user, isRestoringPassword: true);
        var jwt = ReadToken(token);

        Assert.Equal(TruncateToSeconds(_now.UtcDateTime), jwt.ValidFrom);
        Assert.Equal(TruncateToSeconds(_now.AddMinutes(5).UtcDateTime), jwt.ValidTo);
    }

    /// <summary>
    /// Проверяет, что access token подписан алгоритмом RSA SHA-256.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ShouldBeSignedWithRsaSha256()
    {
        var token = _service.GenerateAccessToken(_user);
        var jwt = ReadToken(token);

        Assert.Equal(SecurityAlgorithms.RsaSha256, jwt.Header.Alg);
    }

    /// <summary>
    /// Проверяет, что access token корректно создается, даже если дополнительные claims отсутствуют.
    /// </summary>
    [Fact]
    public void GenerateAccessToken_ShouldWork_WhenCustomClaimsAreNull()
    {
        var user = _user with { Claims = null };

        var token = _service.GenerateAccessToken(user);
        var jwt = ReadToken(token);

        Assert.Contains(jwt.Claims, c => c.Type == "nameid" && c.Value == user.UserId.ToString("D"));
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "scope");
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "tenant");
    }

    /// <summary>
    /// Проверяет, что id token содержит стандартные пользовательские claims.
    /// </summary>
    [Fact]
    public void GenerateIdToken_ShouldContainStandardUserClaims()
    {
        var token = _service.GenerateIdToken(_user);
        var jwt = ReadToken(token);

        Assert.Equal(_settings.AccessTokenSettings.Issuer, jwt.Issuer);
        Assert.Contains(_settings.AccessTokenSettings.Audience, jwt.Audiences);

        Assert.Contains(jwt.Claims, c => c.Type == "nameid" && c.Value == _user.UserId.ToString("D"));
        Assert.Contains(jwt.Claims, c => c.Type == "unique_name" && c.Value == _user.UserName);
        Assert.Contains(jwt.Claims, c => c.Type == "email" && c.Value == _user.Email);
        Assert.Contains(jwt.Claims, c => c.Type == "family_name" && c.Value == _user.LastName);

        var givenNames = jwt.Claims
            .Where(c => c.Type == "given_name")
            .Select(c => c.Value)
            .ToArray();

        Assert.Contains(_user.UserName, givenNames);
        Assert.Contains(_user.FirstName, givenNames);

        Assert.Contains(jwt.Claims, c => c.Type == "scope" && c.Value == "profile");
        Assert.Contains(jwt.Claims, c => c.Type == "tenant" && c.Value == "identity");
    }

    /// <summary>
    /// Проверяет, что id token использует корректный срок жизни.
    /// </summary>
    [Fact]
    public void GenerateIdToken_ShouldUseConfiguredLifetime()
    {
        var token = _service.GenerateIdToken(_user);
        var jwt = ReadToken(token);

        Assert.Equal(TruncateToSeconds(_now.UtcDateTime), jwt.ValidFrom);
        Assert.Equal(TruncateToSeconds(_now.AddSeconds(_settings.AccessTokenSettings.LifeTimeInSeconds).UtcDateTime), jwt.ValidTo);
    }

    /// <summary>
    /// Проверяет, что id token корректно создается, если отсутствуют опциональные поля пользователя.
    /// </summary>
    [Fact]
    public void GenerateIdToken_ShouldOmitOptionalClaims_WhenOptionalFieldsAreMissing()
    {
        var user = new JwtUserData
        {
            UserId = _user.UserId,
            UserName = null,
            Email = null,
            FirstName = null,
            LastName = null,
            Claims = null
        };

        var token = _service.GenerateIdToken(user);
        var jwt = ReadToken(token);

        Assert.Contains(jwt.Claims, c => c.Type == "nameid" && c.Value == user.UserId.ToString("D"));
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);

        Assert.DoesNotContain(jwt.Claims, c => c.Type == "unique_name");
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "email");
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "given_name");
        Assert.DoesNotContain(jwt.Claims, c => c.Type == "family_name");
    }

    /// <summary>
    /// Проверяет, что refresh token генерируется в корректном Base64-формате и имеет ожидаемую длину.
    /// </summary>
    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64StringOfConfiguredLength()
    {
        var token = _service.GenerateRefreshToken();

        Assert.False(string.IsNullOrWhiteSpace(token));

        var bytes = Convert.FromBase64String(token);
        Assert.Equal(_settings.RefreshTokenSettings.Length, bytes.Length);
    }

    /// <summary>
    /// Проверяет, что два refresh token, сгенерированные подряд, отличаются друг от друга.
    /// </summary>
    [Fact]
    public void GenerateRefreshToken_ShouldReturnDifferentValues_OnSubsequentCalls()
    {
        var first = _service.GenerateRefreshToken();
        var second = _service.GenerateRefreshToken();

        Assert.NotEqual(first, second);
    }

    /// <summary>
    /// Проверяет, что срок жизни refresh token возвращается из настроек.
    /// </summary>
    [Fact]
    public void GetRefreshTokenLifetimeInMinutes_ShouldReturnConfiguredValue()
    {
        var lifetime = _service.GetRefreshTokenLifetimeInMinutes();

        Assert.Equal(_settings.RefreshTokenSettings.LifeTimeInMinutes, lifetime);
    }

    /// <summary>
    /// Проверяет, что корректный access token проходит валидацию.
    /// </summary>
    [Fact]
    public void IsTokenValid_ShouldReturnTrue_ForGeneratedAccessToken()
    {
        var token = _service.GenerateAccessToken(_user);

        var result = _service.IsTokenValid(token, validateLifeTime: true);

        Assert.True(result);
    }

    /// <summary>
    /// Проверяет, что пустой, пробельный или некорректный токен отклоняется.
    /// </summary>
    /// <param name="token">Некорректная строка токена.</param>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-jwt")]
    [InlineData("header.payload")]
    public void IsTokenValid_ShouldReturnFalse_ForInvalidToken(string token)
    {
        var result = _service.IsTokenValid(token, validateLifeTime: true);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что токен с истекшим сроком жизни отклоняется.
    /// </summary>
    [Fact]
    public void IsTokenValid_ShouldReturnFalse_ForExpiredToken()
    {
        var expiredToken = CreateToken(
            claims: BuildClaims(includeUserId: true, includeJti: true),
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: _keyProvider.SigningKey,
            notBefore: _now.AddHours(-2).UtcDateTime,
            expires: _now.AddHours(-1).UtcDateTime);

        var result = _service.IsTokenValid(expiredToken, validateLifeTime: true);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что токен с неверным issuer отклоняется.
    /// </summary>
    [Fact]
    public void IsTokenValid_ShouldReturnFalse_ForWrongIssuer()
    {
        var token = CreateToken(
            claims: BuildClaims(includeUserId: true, includeJti: true),
            issuer: "wrong-issuer",
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: _keyProvider.SigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var result = _service.IsTokenValid(token, validateLifeTime: true);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что токен с неверным audience отклоняется.
    /// </summary>
    [Fact]
    public void IsTokenValid_ShouldReturnFalse_ForWrongAudience()
    {
        var token = CreateToken(
            claims: BuildClaims(includeUserId: true, includeJti: true),
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: "wrong-audience",
            signingKey: _keyProvider.SigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var result = _service.IsTokenValid(token, validateLifeTime: true);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что токен, подписанный другим ключом, отклоняется.
    /// </summary>
    [Fact]
    public void IsTokenValid_ShouldReturnFalse_ForInvalidSignature()
    {
        using var otherRsa = RSA.Create(2048);
        var otherSigningKey = new RsaSecurityKey(otherRsa);

        var token = CreateToken(
            claims: BuildClaims(includeUserId: true, includeJti: true),
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: otherSigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var result = _service.IsTokenValid(token, validateLifeTime: true);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что из корректного токена можно извлечь идентификатор пользователя.
    /// </summary>
    [Fact]
    public void GetUserIdFromToken_ShouldReturnUserId()
    {
        var token = _service.GenerateAccessToken(_user);

        var userId = _service.GetUserIdFromToken(token);

        Assert.Equal(_user.UserId, userId);
    }

    /// <summary>
    /// Проверяет, что пустой токен приводит к <see cref="InvalidTokenException"/>.
    /// </summary>
    /// <param name="token">Пустой или пробельный токен.</param>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void GetUserIdFromToken_ShouldThrowInvalidTokenException_WhenTokenIsEmpty(string token)
    {
        var exception = Assert.Throws<InvalidTokenException>(() => _service.GetUserIdFromToken(token));

        Assert.Equal("Token is empty.", exception.Message);
    }

    /// <summary>
    /// Проверяет, что токен без user id приводит к <see cref="InvalidTokenException"/>.
    /// </summary>
    [Fact]
    public void GetUserIdFromToken_ShouldThrowInvalidTokenException_WhenUserIdClaimIsMissing()
    {
        var token = CreateToken(
            claims: BuildClaims(includeUserId: false, includeJti: true),
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: _keyProvider.SigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var exception = Assert.Throws<InvalidTokenException>(() => _service.GetUserIdFromToken(token));

        Assert.Equal("Token does not contain a valid UserId.", exception.Message);
    }

    /// <summary>
    /// Проверяет, что токен с невалидным user id приводит к <see cref="InvalidTokenException"/>.
    /// </summary>
    [Fact]
    public void GetUserIdFromToken_ShouldThrowInvalidTokenException_WhenUserIdClaimIsInvalid()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "not-a-guid"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };

        var token = CreateToken(
            claims: claims,
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: _keyProvider.SigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var exception = Assert.Throws<InvalidTokenException>(() => _service.GetUserIdFromToken(token));

        Assert.Equal("Token does not contain a valid UserId.", exception.Message);
    }

    /// <summary>
    /// Проверяет, что токен, подписанный другим ключом, приводит к <see cref="InvalidTokenException"/>.
    /// </summary>
    [Fact]
    public void GetUserIdFromToken_ShouldThrowInvalidTokenException_WhenSignatureIsInvalid()
    {
        using var otherRsa = RSA.Create(2048);
        var otherSigningKey = new RsaSecurityKey(otherRsa);

        var token = CreateToken(
            claims: BuildClaims(includeUserId: true, includeJti: true),
            issuer: _settings.AccessTokenSettings.Issuer,
            audience: _settings.AccessTokenSettings.Audience,
            signingKey: otherSigningKey,
            notBefore: _now.UtcDateTime,
            expires: _now.AddHours(1).UtcDateTime);

        var exception = Assert.Throws<InvalidTokenException>(() => _service.GetUserIdFromToken(token));

        Assert.Equal("Token is invalid.", exception.Message);
    }

    /// <summary>
    /// Возвращает десериализованный JWT без проверки подписи для анализа claims и сроков жизни.
    /// </summary>
    private static JwtSecurityToken ReadToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(token);
    }

    /// <summary>
    /// Создает JWT с заданными параметрами подписи, issuer, audience и временем жизни.
    /// </summary>
    private static string CreateToken(
        IEnumerable<Claim> claims,
        string issuer,
        string audience,
        RsaSecurityKey signingKey,
        DateTime notBefore,
        DateTime expires)
    {
        var handler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity(claims, "Bearer");
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);

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
    /// Создает набор claims для негативных сценариев, где можно включать или выключать user id и jti.
    /// </summary>
    private static List<Claim> BuildClaims(bool includeUserId, bool includeJti)
    {
        var claims = new List<Claim>();

        if (includeUserId)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.Parse("11111111-1111-1111-1111-111111111111").ToString("D")));
        }

        if (includeJti)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
        }

        return claims;
    }

    /// <summary>
    /// Формирует тестовые настройки JWT.
    /// </summary>
    private static JwtSettings CreateSettings()
    {
        return new JwtSettings
        {
            AccessTokenSettings = new AccessTokenSettings
            {
                Issuer = "identity-api",
                Audience = "identity-client",
                LifeTimeInSeconds = 3600
            },
            RefreshTokenSettings = new RefreshTokenSettings
            {
                Length = 32,
                LifeTimeInMinutes = 1440
            }
        };
    }
    
    /// <summary>
    /// Обрезает значение <see cref="DateTime"/> до точности в секунды,
    /// отбрасывая миллисекунды и более мелкие единицы времени.
    /// </summary>
    /// <param name="value">Исходное значение времени.</param>
    /// <returns>
    /// Новое значение <see cref="DateTime"/>, округленное вниз до ближайшей секунды.
    /// </returns>
    /// <remarks>
    /// Используется в тестах для корректного сравнения времени с JWT-токенами,
    /// так как <see cref="System.IdentityModel.Tokens.Jwt.JwtSecurityToken"/> 
    /// хранит время с точностью до секунд (без миллисекунд).
    /// </remarks>
    private static DateTime TruncateToSeconds(DateTime value)
    {
        return new DateTime(
            value.Ticks - (value.Ticks % TimeSpan.TicksPerSecond),
            value.Kind);
    }

    /// <summary>
    /// Фиксированный поставщик времени для стабильных тестов.
    /// </summary>
    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        /// <summary>
        /// Возвращает заранее заданное UTC-время.
        /// </summary>
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    /// <summary>
    /// Тестовая реализация <see cref="IRsaKeyProvider"/> с общей RSA-парой для подписи и проверки.
    /// </summary>
    private sealed class TestRsaKeyProvider : IRsaKeyProvider
    {
        private readonly RSA _rsa = RSA.Create(2048);

        /// <inheritdoc />
        public RsaSecurityKey SigningKey { get; }

        /// <inheritdoc />
        public RsaSecurityKey ValidationKey { get; }

        /// <summary>
        /// Создает пару ключей на основе одного RSA-экземпляра.
        /// </summary>
        public TestRsaKeyProvider()
        {
            SigningKey = new RsaSecurityKey(_rsa);
            ValidationKey = new RsaSecurityKey(_rsa);
        }

        /// <summary>
        /// Освобождает криптографический ресурс RSA.
        /// </summary>
        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}