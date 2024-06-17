using Nutrifica.Shared.Wrappers;

namespace Nutrifice.Domain.UnitTests;

public class ResultTests
{
    [Fact]
    public void Result_WhenFailed_HasError()
    {
        // Arrange
        var sut = Result.Failure(Error.NullValue);

        // Act

        // Assert
        Assert.True(sut.IsFailure);
        Assert.False(sut.IsSuccess);
        Assert.Equal(sut.Error, Error.NullValue);
    }

    [Fact]
    public void Result_WhenSucceeded_HasNoError()
    {
        // Arrange
        var sut = Result.Success();

        // Act

        // Assert
        Assert.True(sut.IsSuccess);
        Assert.False(sut.IsFailure);
        Assert.Equal(Error.None, sut.Error);
    }

    [Fact]
    public void ResultWithValue_WhenSucceeded_HasNoError()
    {
        var o = new Object();
        // Arrange

        var sut = Result<Object>.Success(o);
        // Act

        // Assert
        Assert.True(sut.IsSuccess);
        Assert.False(sut.IsFailure);
        Assert.Equal(o, sut.Value);
        Assert.Equal(Error.None, sut.Error);
    }

    [Fact]
    public void Result_WhenGetValueOnFailedResult_Throws()
    {
        // Arrange
        var sut = Result.Failure<Object>(Error.NullValue);

        // Act
        var action = () => sut.Value;

        // Assert
        Assert.False(sut.IsSuccess);
        Assert.True(sut.IsFailure);
        Assert.Equal(Error.NullValue, sut.Error);
        Assert.Throws<InvalidOperationException>(action);
    }
}
