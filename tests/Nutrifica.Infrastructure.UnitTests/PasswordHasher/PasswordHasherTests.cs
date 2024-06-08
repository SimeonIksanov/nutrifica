using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Infrastructure.Services;

namespace Nutrifica.Infrastructure.UnitTests.PasswordHasher;

public class PasswordHasherTests
{
    private IPasswordHasherService sut = new PasswordHasherService();
    
    [Fact]
    public void GenerateSalt_Returns_16bytes()
    {
        var salt = sut.GenerateSalt();
        Assert.Equal(salt.Length, 16);
    }

    [Fact]
    public void HashPassword_WhenPasswordProvided_ReturnsHashedPassword()
    {
        var hashedPassword = sut.HashPassword("some string goes here", sut.GenerateSalt());
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public void Verify_WhenProvidedCorrectPassword_ReturnsTrue()
    {
        var initialPassword = "123abc456def";
        var hashedAndSalt = sut.HashPassword(initialPassword);

        var actual = sut.Verify(initialPassword, hashedAndSalt.hashed, hashedAndSalt.salt);
        
        Assert.True(actual);
    }
    
    [Fact]
    public void Verify_WhenProvidedNotCorrectPassword_ReturnsFalse()
    {
        var initialPassword = "123abc456def";
        var badPassword = "123abc456defff";
        var hashedAndSalt = sut.HashPassword(initialPassword);

        var actual = sut.Verify(badPassword, hashedAndSalt.hashed, hashedAndSalt.salt);
        
        Assert.False(actual);
    }
}