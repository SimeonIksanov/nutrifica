
namespace Nutrifica.Application.UnitTests.Authentication.Refresh;

public class RefreshTokensCommandHandlerTests
{

    private readonly RefreshTokensCommandHandler _sut;
    private readonly RefreshTokensCommand _command;

    public RefreshTokensCommandHandlerTests()
    {
        _sut = new RefreshTokensCommandHandler();
    }

    [Fact]
    public async Task Handle_When_ProvidedRefreshTokenNotExists_Should_ReturnNotFound()
    {
        var actual = await _sut.Handle(_command, default);
        Assert.False(actual.IsSuccess);
    }

    [Fact]
    public async Task Handle_When_ExpiredRefreshTokensProvided_Should_ReturnTokenExpired()
    {
        var actual = await _sut.Handle(_command, default);
        Assert.False(actual.IsSuccess);
    }

    [Fact]
    public async Task Handle_When_ValidRefreshTokensProvided_Should_ReturnRefreshedTokenPair()
    {
        var actual = await _sut.Handle(_command, default);
        Assert.True(actual.IsSuccess);
        Assert.NotEmpty(actual.Value.Jwt);
        Assert.NotEmpty(actual.Value.RefreshToken);
    }
}
