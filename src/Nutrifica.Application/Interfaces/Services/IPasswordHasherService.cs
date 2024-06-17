namespace Nutrifica.Application.Interfaces.Services;

public interface IPasswordHasherService
{
    (string hashed, string salt) HashPassword(string password);
    string HashPassword(string password, byte[] salt);
    bool Verify(string providedPassword, string storedHashed, string salt);
    byte[] GenerateSalt();
}