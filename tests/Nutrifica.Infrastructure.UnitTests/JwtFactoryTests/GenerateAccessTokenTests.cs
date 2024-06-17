using Nutrifica.Infrastructure.Clock;
using Nutrifica.Infrastructure.UnitTests.Utilities;

namespace Nutrifica.Infrastructure.UnitTests.JwtFactoryTests;

public class GenerateAccessTokenTests
{
    [Fact]
    public void GenerateAccessToken_Returns_NotEmptyToken()
    {
        // Arrange
        var jwtSettings = JwtSettingsFactory.Create();
        var user = UserCreator.Create();
        var sut = new Authentication.JwtFactory(jwtSettings, new DateTimeProvider());

        // Act
        var actual = sut.GenerateAccessToken(user);

        // Assert
        Assert.NotNull(actual);
        Assert.True(actual.Length > 0);
    }
}
