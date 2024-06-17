using Nutrifica.Application.Authentication.Login;

namespace Nutrifica.Application.UnitTests.Authentication.Login;

public class LoginCommandValidationTests
{
    private readonly LoginCommandValidator _sut = new LoginCommandValidator();

    [Fact]
    public void Validate_WhenValidCommand_ReturnSuccess()
    {
        // Arrange
        var command = new LoginCommand() { Username = "aa", Password = "bb" };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WhenUsernameEmpty_ReturnFailure()
    {
        // Arrange
        var command = new LoginCommand() { Username = string.Empty, Password = "bb" };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenPasswordEmpty_ReturnFailure()
    {
        // Arrange
        var command = new LoginCommand() { Username = "asdf", Password = string.Empty };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }
}
