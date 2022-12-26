using Microsoft.Extensions.Logging;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services;
using Sonab.WebAPI.Utils.Constants;

namespace Sonab.WebAPI.Test.Services;

public class PostServiceTests : BaseServiceSetup
{
    private readonly Mock<IPostRepository> _mockRepository = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepository = new();
    private readonly PostService _service;

    public PostServiceTests()
    {
        _mockRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<Post>()))
            .ReturnsAsync(true);

        _service = new(
            Mock.Of<ILogger<PostService>>(),
            _mockAccessor.Object,
            _mockRepository.Object,
            _mockUserRepository.Object,
            _mockSubscriptionRepository.Object);
    }

    [Fact]
    public async Task GetList()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetAsync(It.IsAny<ListParams>()))
            .ReturnsAsync(new PostShortInfo[5]);
        _mockRepository.Setup(x => x.GetUserPostsAsync(
            It.Is<string>(y => y == "USER1"),
            It.IsAny<ListParams>())).ReturnsAsync(new PostShortInfo[2]);

        // Act
        ServiceResponse result1 = await _service.GetListAsync(SearchType.All, new());
        ServiceResponse result2 = await _service.GetListAsync(SearchType.User, new());

        // Assert
        Assert.Equal(5, ((PostShortInfo[])result1.Data).Length);
        Assert.Equal(2, ((PostShortInfo[])result2.Data).Length);
    }

    [Fact]
    public async Task Count()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.CountAsync())
            .ReturnsAsync(5);
        _mockRepository.Setup(x => x.CountPublishersPostAsync(
            It.Is<string>(y => y == "USER1"))).ReturnsAsync(3);

        // Act
        ServiceResponse result1 = await _service.CountAsync(SearchType.All);
        ServiceResponse result2 = await _service.CountAsync(SearchType.Publishers);

        // Assert
        Assert.Equal(5, (int)result1.Data);
        Assert.Equal(3, (int)result2.Data);
    }

    [Fact]
    public async Task GetById_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User()
            });
        _mockSubscriptionRepository.Setup(x => x.IsSubscribedAsync(
            It.IsAny<User>(),
            It.Is<string>(y => y == "USER1"))).ReturnsAsync(false);

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        PostFullInfo post = (PostFullInfo)result.Data;
        Assert.False(post.IsEditAllowed);
        Assert.False(post.IsSubscribed);
    }

    [Fact]
    public async Task GetById_Anonymous()
    {
        // Setup
        SetUserId(null);
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User()
            });

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        PostFullInfo post = (PostFullInfo)result.Data;
        Assert.False(post.IsEditAllowed);
        Assert.False(post.IsSubscribed);
    }

    [Fact]
    public async Task GetById_Owner()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User
                {
                    ExternalId = "USER1"
                }
            });

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        PostFullInfo post = (PostFullInfo)result.Data;
        Assert.True(post.IsEditAllowed);
        Assert.False(post.IsSubscribed);
    }

    [Fact]
    public async Task GetById_Subscribed()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User()
            });
        _mockSubscriptionRepository.Setup(x => x.IsSubscribedAsync(
            It.IsAny<User>(),
            It.Is<string>(y => y == "USER1"))).ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        PostFullInfo post = (PostFullInfo)result.Data;
        Assert.False(post.IsEditAllowed);
        Assert.True(post.IsSubscribed);
    }

    [Fact]
    public async Task GetById_NotFound()
    {
        // Setup
        SetUserId(null);
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(null as Post);

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Create_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User());
        _mockRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<Post>()))
            .Callback<Post>((p) => p.Id = 7)
            .ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.CreateAsync(new());

        // Assert
        Assert.True(result.IsSuccess());
        Assert.Equal(7, (int)result.Data);
    }

    [Fact]
    public async Task Create_NoUser()
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(null as User);

        // Act
        ServiceResponse result = await _service.CreateAsync(new());

        // Assert
        Assert.Equal(409, result.StatusCode);
        Assert.Contains(Messages.InfoNotLoaded, result.Messages.Errors["Error"]);
    }

    [Fact]
    public async Task Update_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User
                {
                    ExternalId = "USER1"
                }
            });
        bool updated = false;
        _mockRepository.Setup(x => x.UpdateAndSaveAsync(It.IsAny<Post>()))
            .Callback(() => updated = true)
            .ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.UpdateAsync(1, new());

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(updated);
    }

    [Fact]
    public async Task Update_NotFound()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(null as Post);

        // Act
        ServiceResponse result = await _service.UpdateAsync(1, new());

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Update_NoAccess()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User
                {
                    ExternalId = "USER2"
                }
            });
        bool updated = false;
        _mockRepository.Setup(x => x.UpdateAndSaveAsync(It.IsAny<Post>()))
            .Callback(() => updated = true)
            .ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.UpdateAsync(1, new());

        // Assert
        Assert.Equal(403, result.StatusCode);
        Assert.Contains(Messages.OnlyOwner, result.Messages.Errors["Error"]);
        Assert.False(updated);
    }

    [Fact]
    public async Task Remove_Ok()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User
                {
                    ExternalId = "USER1"
                }
            });
        bool deleted = false;
        _mockRepository.Setup(x => x.DeleteAndSaveAsync(It.IsAny<Post>()))
            .Callback(() => deleted = true)
            .ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.RemoveAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(deleted);
    }

    [Fact]
    public async Task Remove_NotFound()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(null as Post);

        // Act
        ServiceResponse result = await _service.RemoveAsync(1);

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Remove_NoAccess()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetFullInfoAsync(It.Is<int>(y => y == 1)))
            .ReturnsAsync(new Post
            {
                User = new User
                {
                    ExternalId = "USER2"
                }
            });
        bool deleted = false;
        _mockRepository.Setup(x => x.DeleteAndSaveAsync(It.IsAny<Post>()))
            .Callback(() => deleted = true)
            .ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.RemoveAsync(1);

        // Assert
        Assert.Equal(403, result.StatusCode);
        Assert.Contains(Messages.OnlyOwner, result.Messages.Errors["Error"]);
        Assert.False(deleted);
    }
}
