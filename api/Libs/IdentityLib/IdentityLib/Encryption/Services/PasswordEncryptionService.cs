using System.Security.Cryptography;
using System.Text;
using IdentityLib.Encryption.Interfaces;
using Konscious.Security.Cryptography;

namespace IdentityLib.Encryption.Services;

/// <inheritdoc />
public class PasswordEncryptionService : IPasswordEncryptionService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;

    private const int Iterations = 2;
    private const int MemorySize = 19456;
    private const int Parallelism = 1;

    /// <inheritdoc />
    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = Iterations,
            MemorySize = MemorySize,
            DegreeOfParallelism = Parallelism
        };

        var hash = argon2.GetBytes(HashSize);

        return string.Join(
            ':',
            "argon2id",
            Iterations,
            MemorySize,
            Parallelism,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    /// <inheritdoc />
    public bool VerifyPassword(string password, string storedHash)
    {
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(storedHash);

        var parts = storedHash.Split(':');
        if (parts.Length != 6 || !parts[0].Equals("argon2id", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var iterations) ||
            !int.TryParse(parts[2], out var memorySize) ||
            !int.TryParse(parts[3], out var parallelism))
        {
            return false;
        }

        byte[] salt;
        byte[] expectedHash;

        try
        {
            salt = Convert.FromBase64String(parts[4]);
            expectedHash = Convert.FromBase64String(parts[5]);
        }
        catch
        {
            return false;
        }

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = iterations,
            MemorySize = memorySize,
            DegreeOfParallelism = parallelism
        };

        var actualHash = argon2.GetBytes(expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}