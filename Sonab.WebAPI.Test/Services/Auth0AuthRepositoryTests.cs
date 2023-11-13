using Microsoft.Extensions.Logging;
using Sonab.Auth0Client;
using Sonab.Auth0Client.Models;
using Sonab.Core.Interfaces.Services;

namespace Sonab.WebAPI.Test.Services;

// TODO: Move to new project
public class Auth0AuthRepositoryTests
{
    private readonly Auth0Options _options = new("", "", "http://Auth0.Some/", "id", "secret");
    private readonly Mock<IRequestClient> _mockRequestClient = new();
    private readonly Auth0AuthRepository _service;

    public Auth0AuthRepositoryTests()
    {
        _mockRequestClient.Setup(x => x.GetRequestAsync<UserData>(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new UserData
            {
                Email = "some@mail.com",
                UserName = "Some.Fancy"
            });

        _service = new(
            Mock.Of<ILogger<Auth0AuthRepository>>(),
            _options,
            _mockRequestClient.Object);
    }

    [Fact]
    public async Task GetUserInfo_Ok()
    {
        // Setup
        _mockRequestClient.Setup(x =>
                x.PostRequestAsync<TokenData>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new TokenData
            {
                AccessToken = "token",
                ExpiresIn = 500,
                Scope = "read:users"
            });

        // Act
        var result = await _service.GetUserInfoAsync("user1");

        // Assert
        Assert.Equal("Some.Fancy", result.UserName);
    }

    [Fact]
    public async Task GetUserInfo_TwoTimes()
    {
        // Setup
        var sentCount = 0;
        _mockRequestClient.Setup(x =>
                x.PostRequestAsync<TokenData>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Callback(() => sentCount++)
            .ReturnsAsync(new TokenData
            {
                AccessToken = "token",
                ExpiresIn = 500,
                Scope = "read:users"
            });
        var service = new Auth0AuthRepository(
            Mock.Of<ILogger<Auth0AuthRepository>>(),
            _options,
            _mockRequestClient.Object
        );
        await service.GetUserInfoAsync("user1");

        // Act
        var result = await service.GetUserInfoAsync("user1");

        // Assert
        Assert.Equal(1, sentCount);
        Assert.Equal("Some.Fancy", result.UserName);
    }
}
