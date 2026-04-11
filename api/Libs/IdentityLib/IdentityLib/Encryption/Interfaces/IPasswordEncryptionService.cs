namespace IdentityLib.Encryption.Interfaces;

/// <summary>
/// Сервис шифрования паролей.
/// </summary>
public interface IPasswordEncryptionService
{
    /// <summary>
    /// Хэширует пароль и возвращает самодостаточную строку:
    /// algorithm:iterations:memory:parallelism:salt:hash
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Проверяет пароль относительно сохраненной строки-хэша.
    /// </summary>
    bool VerifyPassword(string password, string storedHash);
}