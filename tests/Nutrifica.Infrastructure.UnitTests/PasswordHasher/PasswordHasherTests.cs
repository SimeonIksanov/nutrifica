using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Infrastructure.Services;

namespace Nutrifica.Infrastructure.UnitTests.PasswordHasher;

public class PasswordHasherTests
{
    private readonly IPasswordHasherService _sut = new PasswordHasherService();

    [Fact]
    public void GenerateSalt_Returns_16bytes()
    {
        var salt = _sut.GenerateSalt();
        Assert.Equal(16, salt.Length);
    }

    [Fact]
    public void HashPassword_WhenPasswordProvided_ReturnsHashedPassword()
    {
        var hashedPassword = _sut.HashPassword("some string goes here", _sut.GenerateSalt());
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public void Verify_WhenProvidedCorrectPassword_ReturnsTrue()
    {
        var initialPassword = "123abc456def";
        var hashedAndSalt = _sut.HashPassword(initialPassword);

        var actual = _sut.Verify(initialPassword, hashedAndSalt.hashed, hashedAndSalt.salt);

        Assert.True(actual);
    }

    [Fact]
    public void Verify_WhenProvidedNotCorrectPassword_ReturnsFalse()
    {
        var initialPassword = "123abc456def";
        var badPassword = "123abc456defff";
        var hashedAndSalt = _sut.HashPassword(initialPassword);

        var actual = _sut.Verify(badPassword, hashedAndSalt.hashed, hashedAndSalt.salt);

        Assert.False(actual);
    }
}
