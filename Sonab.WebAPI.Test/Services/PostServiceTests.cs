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
    private readonly Mock<ITopicRepository> _mockTopicRepository = new();
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
            _mockSubscriptionRepository.Object,
            _mockTopicRepository.Object);
    }

    [Fact]
    public async Task GetList()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.GetAsync(It.IsAny<PostListParams>()))
            .ReturnsAsync(new List<PostShortInfo>
            {
                new PostShortInfo(),
                new PostShortInfo(),
                new PostShortInfo(),
                new PostShortInfo(),
                new PostShortInfo()
            });
        _mockRepository.Setup(x => x.GetUserPostsAsync(
            It.Is<string>(y => y == "USER1"),
            It.IsAny<PostListParams>())
        )
            .ReturnsAsync(new List<PostShortInfo>
            {
                new PostShortInfo(),
                new PostShortInfo()
            });

        // Act
        ServiceResponse result1 = await _service.GetListAsync(SearchType.All, new());
        ServiceResponse result2 = await _service.GetListAsync(SearchType.User, new());

        // Assert
        Assert.True(result1.TryGetData(out List<PostShortInfo> data1));
        Assert.Equal(5, data1.Count);
        Assert.True(result2.TryGetData(out List<PostShortInfo> data2));
        Assert.Equal(2, data2.Count);
    }

    [Fact]
    public async Task Count()
    {
        // Setup
        SetUserId("user1");
        _mockRepository.Setup(x => x.CountAsync(It.IsAny<PostCountParams>()))
            .ReturnsAsync(5);
        _mockRepository.Setup(x => x.CountPublishersPostAsync(
            It.Is<string>(y => y == "USER1"),
            It.IsAny<PostCountParams>())
        )
            .ReturnsAsync(3);

        // Act
        ServiceResponse result1 = await _service.CountAsync(SearchType.All, new());
        ServiceResponse result2 = await _service.CountAsync(SearchType.Publishers, new());

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
                User = new User(),
                Topics = new()
            });
        _mockSubscriptionRepository.Setup(x => x.IsSubscribedAsync(
            It.IsAny<User>(),
            It.Is<string>(y => y == "USER1"))).ReturnsAsync(false);

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(result.TryGetData(out PostFullInfo post));
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
                User = new User(),
                Topics = new()
            });

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(result.TryGetData(out PostFullInfo post));
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
                },
                Topics = new()
            });

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(result.TryGetData(out PostFullInfo post));
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
                User = new User(),
                Topics = new()
            });
        _mockSubscriptionRepository.Setup(x => x.IsSubscribedAsync(
            It.IsAny<User>(),
            It.Is<string>(y => y == "USER1"))).ReturnsAsync(true);

        // Act
        ServiceResponse result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(result.TryGetData(out PostFullInfo post));
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

    [Theory]
    [MemberData(nameof(RequestsWithStoredTags))]
    public async Task Create_Ok(EditRequest request)
    {
        // Setup
        int savedTagCount = 0;
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User());
        _mockRepository.Setup(x => x.AddAndSaveAsync(It.IsAny<Post>()))
            .Callback<Post>((p) =>
            {
                p.Id = 7;
                savedTagCount = p.Topics.Count;
            })
            .ReturnsAsync(true);
        _mockTopicRepository.Setup(x => x.GetAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((IEnumerable<int> x) => x.Select(y => new Topic
            {
                Id = y
            }).ToList());

        // Act
        ServiceResponse result = await _service.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(result.TryGetData(out int id));
        Assert.Equal(7, id);
        Assert.Equal(request.Tags?.Length ?? 0, savedTagCount);
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

    [Theory]
    [MemberData(nameof(RequestsWithMissingTags))]
    public async Task Create_TagsNotFound(
        EditRequest request,
        int[] existing,
        string[] expectedFields)
    {
        // Setup
        SetUserId("user1");
        _mockUserRepository.Setup(x => x.GetByExternalIdAsync(It.Is<string>(y => y == "USER1")))
            .ReturnsAsync(new User());
        _mockTopicRepository.Setup(x => x.GetAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((IEnumerable<int> x) => x
                .Where(y => existing.Contains(y)).Select(y => new Topic
                {
                    Id = y
                }).ToList());

        // Act
        ServiceResponse result = await _service.CreateAsync(request);

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expectedFields, result.Messages.Errors.Keys);
    }

    [Theory]
    [MemberData(nameof(RequestsWithStoredTags))]
    public async Task Update_Ok(EditRequest request)
    {
        // Setup
        int savedTagCount = 0;
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
            .Callback<Post>((p) =>
            {
                updated = true;
                savedTagCount = p.Topics.Count;
            })
            .ReturnsAsync(true);
        _mockTopicRepository.Setup(x => x.GetAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((IEnumerable<int> x) => x.Select(y => new Topic
            {
                Id = y
            }).ToList());

        // Act
        ServiceResponse result = await _service.UpdateAsync(1, request);

        // Assert
        Assert.True(result.IsSuccess());
        Assert.True(updated);
        Assert.Equal(request.Tags?.Length ?? 0, savedTagCount);
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

    [Theory]
    [MemberData(nameof(RequestsWithMissingTags))]
    public async Task Update_TagsNotFound(
        EditRequest request,
        int[] existing,
        string[] expectedFields
    )
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
        _mockTopicRepository.Setup(x => x.GetAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync((IEnumerable<int> x) => x
                .Where(y => existing.Contains(y)).Select(y => new Topic
                {
                    Id = y
                }).ToList());

        // Act
        ServiceResponse result = await _service.UpdateAsync(1, request);

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(expectedFields, result.Messages.Errors.Keys);
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

    public static IEnumerable<object[]> RequestsWithStoredTags => new List<object[]>
    {
        new object[]
        {
            new EditRequest()
        },
        new object[]
        {
            new EditRequest
            {
                Tags = new TopicTag[0]
            },
        },
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag
                    {
                        Id = 1
                    },
                    new TopicTag
                    {
                        Name = "Fancy"
                    },
                    new TopicTag
                    {
                        Id = 2,
                        Name = "Test"
                    }
                }
            }
        },
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag
                    {
                        Name = "Fancy"
                    },
                    new TopicTag
                    {
                        Name = "Test"
                    }
                }
            }
        },
    };

    public static IEnumerable<object[]> RequestsWithMissingTags => new List<object[]>
    {
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag
                    {
                        Id = 2
                    },
                    new TopicTag
                    {
                        Id = 3
                    }
                }
            },
            new int[0],
            new[] { "Tags[0].Id", "Tags[1].Id" }
        },
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag
                    {
                        Id = 2
                    },
                    new TopicTag
                    {
                        Id = 3
                    },
                    new TopicTag
                    {
                        Id = 4
                    }
                }
            },
            new[] { 2 },
            new[] { "Tags[1].Id", "Tags[2].Id" }
        },
    };
}
