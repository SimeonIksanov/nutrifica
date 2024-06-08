using Nutrifica.Shared.Wrappers;

namespace Nutrifice.Domain.UnitTests;

public class ResultTests
{
    [Fact]
    public void Result_WhenFailed_HasError()
    {
        // Arrange
        var sut = Result.Fail("fail");

        // Act

        // Assert
        Assert.True(sut.Failure);
        Assert.False(sut.Success);
        Assert.Equal("fail", sut.Message);
    }

    [Fact]
    public void Result_WhenSucceeded_HasNoError()
    {
        // Arrange
        var sut = Result.Ok();

        // Act

        // Assert
        Assert.True(sut.Success);
        Assert.False(sut.Failure);
        Assert.True(string.IsNullOrEmpty(sut.Message));
    }

    [Fact]
    public void ResultWithValue_WhenSucceeded_HasNoError()
    {
        var o = new Object();
        // Arrange

        var sut = Result<Object>.Ok(o);
        // Act

        // Assert
        Assert.True(sut.Success);
        Assert.False(sut.Failure);
        Assert.Equal(o, sut.Value);
        Assert.True(string.IsNullOrEmpty(sut.Message));
    }

    [Fact]
    public void Result_WhenFailed_ThrowsIfTryGetValue()
    {
        // Arrange
        var sut = Result<Object>.Fail("fail");

        // Act
        var action = () => sut.Value;

        // Assert
        Assert.False(sut.Success);
        Assert.True(sut.Failure);
        Assert.Equal("fail", sut.Message);
        Assert.Throws<InvalidOperationException>(action);
    }
}