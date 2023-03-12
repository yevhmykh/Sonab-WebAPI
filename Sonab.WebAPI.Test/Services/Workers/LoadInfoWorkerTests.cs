using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Hubs;
using Sonab.WebAPI.Models.Auth0Communication;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Services.Background.Workers;

namespace Sonab.WebAPI.Test.Services.Workers;

public class LoadInfoWorkerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<IAuth0CommunicationService> _mockAuth0Service = new();
    private readonly Mock<IHubClients> _mockClients = new();
    private readonly IClientProxy _clientProxy;
    private readonly LoadInfoWorker _worker;

    public LoadInfoWorkerTests()
    {
        _mockUserRepository.Setup(x => x.UpdateAndSaveAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        Mock<IClientProxy> mockClientProxy = new();
        mockClientProxy.Setup(x => x.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object[]>(),
            It.IsAny<CancellationToken>()));
        _clientProxy = mockClientProxy.Object;
        Mock<IHubContext<NotificationHub>> mockHub = new();
        mockHub.Setup(x => x.Clients).Returns(_mockClients.Object);

        _worker = new(
            Mock.Of<ILogger<LoadInfoWorker>>(),
            _mockUserRepository.Object,
            _mockAuth0Service.Object,
            mockHub.Object
        );
    }

    [Fact]
    public async Task LoadNewUser_Ok()
    {
        // Setup
        User user = null;
        _mockUserRepository.Setup(x => x.GetByEmailAsync(It.Is<string>(y => y.ToUpper() == "S@S.S")))
            .ReturnsAsync(null as User);
        _mockAuth0Service.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserInfo()
            {
                UserName = "Weaboo",
                Email = "s@s.s"
            });
        _mockUserRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<User>()))
            .Callback<User>((u) => user = u)
            .ReturnsAsync(true);

        // Act
        await _worker.StartWork("aBc123", new CancellationToken());

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Weaboo", user.Name);
        Assert.Equal("S@S.S", user.Email);
        Assert.Equal("ABC123", user.ExternalId);
    }

    [Fact]
    public async Task LoadNewUser_Fail()
    {
        // Setup
        bool sent = false;
        _mockUserRepository.Setup(x => x.GetByEmailAsync(It.Is<string>(y => y.ToUpper() == "S@S.S")))
            .ReturnsAsync(null as User);
        _mockAuth0Service.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserInfo()
            {
                UserName = "Weaboo",
                Email = "s@s.s"
            });
        _mockUserRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception());
        _mockClients.Setup(x => x.User(It.Is<string>(y => y == "aBc123")))
            .Callback(() => sent = true)
            .Returns(_clientProxy);

        // Act
        try
        {
            await _worker.StartWork("aBc123", new CancellationToken());
        }
        catch { }

        // Assert
        Assert.True(sent);
    }

    [Fact]
    public async Task RefreshName()
    {
        // Setup
        User user = new()
        {
            Name = "Neet",
            ExternalId = "ABC123"
        };
        _mockUserRepository.Setup(x => x.GetByEmailAsync(It.Is<string>(y => y.ToUpper() == "S@S.S")))
            .ReturnsAsync(user);
        _mockAuth0Service.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserInfo()
            {
                UserName = "Weaboo",
                Email = "s@s.s"
            });

        // Act
        await _worker.StartWork("aBc123", new CancellationToken());

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Weaboo", user.Name);
        Assert.Equal("ABC123", user.ExternalId);
    }

    [Fact]
    public async Task RefreshExternalId()
    {
        // Setup
        User user = new()
        {
            Name = "Weaboo",
            ExternalId = "Auth0|123"
        };
        _mockUserRepository.Setup(x => x.GetByEmailAsync(It.Is<string>(y => y.ToUpper() == "S@S.S")))
            .ReturnsAsync(user);
        _mockAuth0Service.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserInfo()
            {
                UserName = "Weaboo",
                Email = "s@s.s"
            });

        // Act
        await _worker.StartWork("google|543", new CancellationToken());

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Weaboo", user.Name);
        Assert.Equal("GOOGLE|543", user.ExternalId);
    }
}
