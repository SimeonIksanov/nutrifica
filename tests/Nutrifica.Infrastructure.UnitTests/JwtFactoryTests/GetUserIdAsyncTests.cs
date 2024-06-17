using Nutrifica.Infrastructure.Authentication;
using Nutrifica.Infrastructure.Clock;
using Nutrifica.Infrastructure.UnitTests.Utilities;

namespace Nutrifica.Infrastructure.UnitTests.JwtFactoryTests;

public class GetUserIdAsyncTests
{
    [Fact]
    public async void GetUserId_WhenValidJwtProvided_Should_ReturnUserId()
    {
        // Arrange
        var jwtSettings = JwtSettingsFactory.Create();
        var sut = new JwtFactory(jwtSettings, new DateTimeProvider());
        var jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIyMzM4ZDNjMS05MWViLTQxYjQtODhmNi01ZGZiNTE3OTc0MzkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InVzZXJuYW1lIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImZuIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6Imxhc3RuYW1lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsIm5iZiI6MTcxODQ2MDI1MiwiZXhwIjoxNzE4NDYwNTUyLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.iyrtP_5pFMT82mbeujcQjay8q4J_R5xpnkRh29C_OH4";

        // Act
        var actual = await sut.GetUserIdAsync(jwt);

        // Assert
        Assert.False(actual.IsEmpty);
    }
    [Fact]
    public async void GetUserId_WhenInvalidJwtProvided_Should_ReturnEmptyUserId()
    {
        // Arrange
        var jwtSettings = JwtSettingsFactory.Create();
        var sut = new JwtFactory(jwtSettings, new DateTimeProvider());
        var jwt = "bad.jwt.provided";

        // Act
        var actual = await sut.GetUserIdAsync(jwt);

        // Assert
        Assert.True(actual.IsEmpty);
    }
}
