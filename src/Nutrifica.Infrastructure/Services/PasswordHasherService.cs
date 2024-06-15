using System.Security.Cryptography;
using System.Text;
using Nutrifica.Application.Interfaces.Services;

namespace Nutrifica.Infrastructure.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private const int SaltLength = 16;

    public (string hashed, string salt) HashPassword(string password)
    {
        var salt = GenerateSalt();
        var hashed = HashPassword(password, salt);
        return (hashed, Convert.ToBase64String(salt));
    }

    public string HashPassword(string password, byte[] salt)
    {
        using var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = ConcatByteArrays(passwordBytes, salt);

        byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

        return Convert.ToBase64String(hashedBytes);
    }

    public bool Verify(string providedPassword, string storedHashed, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        return HashPassword(providedPassword, saltBytes).Equals(storedHashed);
    }

    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private byte[] ConcatByteArrays(byte[] left, byte[] right)
    {
        var concated = new byte[left.Length + right.Length];
        Buffer.BlockCopy(left, 0, concated, 0, left.Length);
        Buffer.BlockCopy(right, 0, concated, left.Length - 1, right.Length);
        return concated;
    }
}
