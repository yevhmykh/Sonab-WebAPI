using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Subscriptions;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services;
using Sonab.WebAPI.Utils.Constants;

namespace Sonab.WebAPI.Test.Services;

public class SubsriptionServiceTests : BaseServiceSetup
{
    private readonly Mock<ISubscriptionRepository> _mockRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly SubsriptionService _service;

    public SubsriptionServiceTests()
    {
        _mockRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<UserSubscription>()))
            .ReturnsAsync(true);
        _mockRepository.Setup(x => x.DeleteAndSaveAsync(It.IsAny<UserSubscription>()))
            .ReturnsAsync(true);

        _service = new(
            Mock.Of<ILogger<SubsriptionService>>(),
            _mockAccessor.Object,
            _mockRepository.Object,
            _mockUserRepository.Object);
    }

    [Fact]
    public async Task Get()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetByAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new List<SubscriptionFullInfo>
            {
                new SubscriptionFullInfo(),
                new SubscriptionFullInfo(),
                new SubscriptionFullInfo(),
                new SubscriptionFullInfo(),
                new SubscriptionFullInfo()
            });

        // Act
        ServiceResponse result = await _service.GetAsync();

        // Assert
        Assert.True(result.TryGetData(out List<SubscriptionFullInfo> data));
        Assert.Equal(5, data.Count);
    }

    [Fact]
    public async Task Subscribe_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User()
            {
                Id = 1
            });
        _mockUserRepository.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == 2)))
            .ReturnsAsync(new User
            {
                Id = 2
            });
        _mockRepository.Setup(x => x.IsExistsAsync(It.Is<UserSubscription>(
            y => y.User.Id == 1 && y.Publisher.Id == 2))).ReturnsAsync(false);

        // Act
        ServiceResponse result = await _service.SubscribeAsync(2);

        // Assert
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public async Task Subscribe_InfoNotLoaded()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(null as User);

        // Act
        ServiceResponse result = await _service.SubscribeAsync(2);

        // Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Contains(Messages.InfoNotLoaded, result.Messages.Errors["Error"]);
    }

    [Fact]
    public async Task Subscribe_NoPublisher()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User()
            {
                Id = 1
            });
        _mockUserRepository.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == 2)))
            .ReturnsAsync(null as User);

        // Act
        ServiceResponse result = await _service.SubscribeAsync(2);

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Subscribe_AlreadySubcribed()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User()
            {
                Id = 1
            });
        _mockUserRepository.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == 2)))
            .ReturnsAsync(new User
            {
                Id = 2
            });
        _mockRepository.Setup(x => x.IsExistsAsync(It.Is<UserSubscription>(
            y => y.User.Id == 1 && y.Publisher.Id == 2))).ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.SubscribeAsync(2);

        // Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Contains(Messages.AlreadySubscribed, result.Messages.Errors["Error"]);
    }

    [Fact]
    public async Task Unsubscribe_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetSubscriptionAsync(
            It.Is<string>(y => y == "USER1"),
            It.Is<int>(y => y == 2))).ReturnsAsync(new UserSubscription());

        // Act
        ServiceResponse result = await _service.UnsubscribeAsync(2);

        // Assert
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public async Task Unsubscribe_NoSubscription()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetSubscriptionAsync(
            It.Is<string>(y => y == "USER1"),
            It.Is<int>(y => y == 2))).ReturnsAsync(null as UserSubscription);

        // Act
        ServiceResponse result = await _service.UnsubscribeAsync(2);

        // Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Contains(Messages.NotSubscribed, result.Messages.Errors["Error"]);
    }
}
