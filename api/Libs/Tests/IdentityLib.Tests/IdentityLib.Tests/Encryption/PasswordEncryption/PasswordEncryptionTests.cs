using IdentityLib.Encryption.Services;
using Xunit;

namespace IdentityLib.Tests.Encryption.PasswordEncryption;

/// <summary>
/// Набор unit-тестов для <see cref="PasswordEncryptionService"/>.
/// Проверяет формат хеша, корректность верификации, обработку некорректных данных и защитное поведение при ошибочных входах.
/// </summary>
public class PasswordEncryptionTests
{
    private readonly PasswordEncryptionService _service = new();

    /// <summary>
    /// Проверяет, что <see cref="PasswordEncryptionService.HashPassword"/> возвращает хеш
    /// в ожидаемом формате Argon2id и заполняет все части строки.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldReturnValidFormattedHash()
    {
        var password = "password123";

        var hash = _service.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));

        var parts = hash.Split(':');

        Assert.Equal(6, parts.Length);
        Assert.Equal("argon2id", parts[0]);

        Assert.True(int.TryParse(parts[1], out _));
        Assert.True(int.TryParse(parts[2], out _));
        Assert.True(int.TryParse(parts[3], out _));

        Assert.False(string.IsNullOrWhiteSpace(parts[4]));
        Assert.False(string.IsNullOrWhiteSpace(parts[5]));
    }

    /// <summary>
    /// Проверяет, что корректный пароль успешно проходит проверку через <see cref="PasswordEncryptionService.VerifyPassword"/>.
    /// </summary>
    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        var password = "securePassword!";
        var hash = _service.HashPassword(password);

        var result = _service.VerifyPassword(password, hash);

        Assert.True(result);
    }

    /// <summary>
    /// Проверяет, что один и тот же пароль при повторном хешировании дает разные значения за счет новой соли.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldGenerateDifferentHashes_ForSamePassword()
    {
        var password = "samePassword";

        var first = _service.HashPassword(password);
        var second = _service.HashPassword(password);

        Assert.NotEqual(first, second);
    }

    /// <summary>
    /// Проверяет, что неверный пароль не проходит верификацию.
    /// </summary>
    /// <param name="wrongPassword">Неверный пароль.</param>
    [Theory]
    [InlineData("wrongPassword")]
    [InlineData("anotherWrongPassword")]
    public void VerifyPassword_ShouldReturnFalse_ForWrongPassword(string wrongPassword)
    {
        var hash = _service.HashPassword("correctPassword");

        var result = _service.VerifyPassword(wrongPassword, hash);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что строка хеша с неверным форматом отклоняется.
    /// </summary>
    /// <param name="invalidHash">Некорректная строка хеша.</param>
    [Theory]
    [InlineData("invalid-format")]
    [InlineData("bcrypt:2:19456:1:YWJjZGVmZw==:aGVsbG93b3JsZA==")]
    [InlineData("argon2id:bad:format")]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidHash(string invalidHash)
    {
        var result = _service.VerifyPassword("password", invalidHash);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что поврежденные Base64-данные в строке хеша приводят к отказу в проверке.
    /// </summary>
    /// <param name="badHash">Строка хеша с некорректными Base64-значениями.</param>
    [Theory]
    [InlineData("argon2id:2:19456:1:!!!:###")]
    [InlineData("argon2id:2:19456:1:notbase64:alsoinvalid")]
    public void VerifyPassword_ShouldReturnFalse_ForCorruptedBase64(string badHash)
    {
        var result = _service.VerifyPassword("password", badHash);

        Assert.False(result);
    }

    /// <summary>
    /// Проверяет, что <see cref="PasswordEncryptionService.HashPassword"/> выбрасывает
    /// <see cref="ArgumentNullException"/> при передаче <c>null</c>.
    /// </summary>
    [Fact]
    public void HashPassword_ShouldThrowArgumentNullException_WhenPasswordIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _service.HashPassword(null!));
    }

    /// <summary>
    /// Проверяет, что <see cref="PasswordEncryptionService.VerifyPassword"/> выбрасывает
    /// <see cref="ArgumentNullException"/> при передаче <c>null</c> в параметр password.
    /// </summary>
    [Theory]
    [InlineData(null, "hash")]
    public void VerifyPassword_ShouldThrowArgumentNullException_WhenPasswordIsNull(string? password, string hash)
    {
        Assert.Throws<ArgumentNullException>(() => _service.VerifyPassword(password!, hash));
    }

    /// <summary>
    /// Проверяет, что <see cref="PasswordEncryptionService.VerifyPassword"/> выбрасывает
    /// <see cref="ArgumentNullException"/> при передаче <c>null</c> в параметр storedHash.
    /// </summary>
    [Theory]
    [InlineData("password", null)]
    public void VerifyPassword_ShouldThrowArgumentNullException_WhenStoredHashIsNull(string password, string? hash)
    {
        Assert.Throws<ArgumentNullException>(() => _service.VerifyPassword(password, hash!));
    }
}