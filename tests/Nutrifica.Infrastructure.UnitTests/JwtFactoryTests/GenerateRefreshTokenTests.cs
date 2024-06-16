using Nutrifica.Infrastructure.Clock;
using Nutrifica.Infrastructure.UnitTests.Utilities;

namespace Nutrifica.Infrastructure.UnitTests.JwtFactoryTests;

public class GenerateRefreshTokenTests
{
    [Fact]
    public void GenerateRefreshToken_Returns_NotEmptyToken()
    {
        // Arrange 
        var jwtSettings = JwtSettingsFactory.Create();
        var sut = new Authentication.JwtFactory(jwtSettings, new DateTimeProvider());

        // Act
        var actual = sut.GenerateRefreshToken("1.1.1.1");

        // Assert
        Assert.True(actual.Token.Length > 0);
    }
}