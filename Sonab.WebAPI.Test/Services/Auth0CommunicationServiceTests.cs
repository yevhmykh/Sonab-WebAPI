using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Models.Auth0Communication;
using Sonab.WebAPI.Services.Auth0Communication;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Test.Services;

public class Auth0CommunicationServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration = new();
    private readonly Mock<IRequestClient> _mockRequestClient = new();
    private readonly Auth0CommunicationService _service;

    public Auth0CommunicationServiceTests()
    {
        _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Auth0:Domain")]).Returns("http://Auth0.Some/");
        _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Auth0:ClientId")]).Returns("id");
        _mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Auth0:ClientSecret")]).Returns("secret");
        _mockRequestClient.Setup(x => x.GetRequestAsync<UserInfo>(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new UserInfo
            {
                Email = "some@mail.com",
                UserName = "Some.Fancy"
            });

        _service = new(
            Mock.Of<ILogger<Auth0CommunicationService>>(),
            _mockConfiguration.Object,
            _mockRequestClient.Object);
    }

    [Fact]
    public async Task GetUserInfo_Ok()
    {
        // Setup
        _mockRequestClient.Setup(x => x.PostRequestAsync<TokenData>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
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
        _mockRequestClient.Setup(x => x.PostRequestAsync<TokenData>(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Callback(() => sentCount++)
            .ReturnsAsync(new TokenData
            {
                AccessToken = "token",
                ExpiresIn = 500,
                Scope = "read:users"
            });
        var service = new Auth0CommunicationService(
            Mock.Of<ILogger<Auth0CommunicationService>>(),
            _mockConfiguration.Object,
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
