using FluentValidation.TestHelper;

using Nutrifica.Application.Accounts.Update;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.UnitTests.Accounts.Update;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _sut = new UpdateUserCommandValidator();

    [Fact]
    public void Validate_When_IdNull_Should_Fail()
    {
        // Arrange
        UpdateUserCommand command = CreateValidCommand() with { Id = null };

        // Act
        var result = _sut.Validate(command);
        // var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_When_IdEmpty_Should_Fail()
    {
        // Arrange
        UpdateUserCommand command = CreateValidCommand() with { Id = UserId.Empty };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_When_DisableAndNoReasonSpecified_Should_Fail()
    {
        // Arrange
        UpdateUserCommand command = CreateValidCommand() with { Enabled = false, DisableReason = "" };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_When_DisableAndReasonSpecified_Should_Success()
    {
        // Arrange
        UpdateUserCommand command = CreateValidCommand() with { Enabled = false, DisableReason = "some reason" };

        // Act
        var result = _sut.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    private UpdateUserCommand CreateValidCommand()
    {
        var command = new UpdateUserCommand(
            Id: UserId.CreateUnique(),
            Username: "un",
            FirstName: FirstName.Create("fn"),
            MiddleName: MiddleName.Create("mn"),
            LastName: LastName.Create("ln"),
            Email: Email.Create("email"),
            PhoneNumber: PhoneNumber.Create("111"),
            Role: UserRole.Operator,
            Enabled: true,
            DisableReason: "",
            SupervisorId: null);
        return command;
    }
}