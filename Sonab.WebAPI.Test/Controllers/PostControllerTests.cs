using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.TopicTags;
using Sonab.WebAPI.Controllers;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Test.Controllers;

public class PostControllerTests
{
    private readonly Mock<HttpContext> _mockContext = new(MockBehavior.Strict);
    private readonly Mock<IPostService> _mockService = new();
    private readonly PostController _controller;

    public PostControllerTests()
    {
        _controller = new(Mock.Of<ILogger<PostController>>(), _mockService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = _mockContext.Object
        };
    }

    [Fact]
    public async Task Get_All_Unauthorized()
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);
        _mockService.Setup(x => x.GetListAsync(
            It.Is<PostSearchType>(y => y == PostSearchType.All),
            It.IsAny<PostListParams>())).ReturnsAsync(ServiceResponse.CreateOk(new object()));

        // Act
        IActionResult result = await _controller.GetAsync(PostSearchType.All, new());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData(PostSearchType.User)]
    [InlineData(PostSearchType.Publishers)]
    public async Task Get_Unauthorized(PostSearchType searchType)
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);

        // Act
        IActionResult result = await _controller.GetAsync(searchType, new());

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Theory]
    [InlineData(PostSearchType.All)]
    [InlineData(PostSearchType.User)]
    [InlineData(PostSearchType.Publishers)]
    public async Task Get_Authorized(PostSearchType searchType)
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(true);
        _mockService.Setup(x => x.GetListAsync(
            It.Is<PostSearchType>(y => y == searchType),
            It.IsAny<PostListParams>())
        )
            .ReturnsAsync(ServiceResponse.CreateOk(new object()));

        // Act
        IActionResult result = await _controller.GetAsync(searchType, new());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Count_All_Unauthorized()
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);
        _mockService.Setup(x => x.CountAsync(
            It.Is<PostSearchType>(y => y == PostSearchType.All),
            It.IsAny<PostCountParams>())
        )
            .ReturnsAsync(ServiceResponse.CreateOk(1));

        // Act
        IActionResult result = await _controller.CountAsync(PostSearchType.All, new());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData(PostSearchType.User)]
    [InlineData(PostSearchType.Publishers)]
    public async Task Count_Unauthorized(PostSearchType searchType)
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(false);

        // Act
        IActionResult result = await _controller.CountAsync(searchType, new());

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Theory]
    [InlineData(PostSearchType.All)]
    [InlineData(PostSearchType.User)]
    [InlineData(PostSearchType.Publishers)]
    public async Task Count_Authorized(PostSearchType searchType)
    {
        // Setup
        _mockContext.SetupGet(x => x.User.Identity.IsAuthenticated).Returns(true);
        _mockService.Setup(x => x.CountAsync(
            It.Is<PostSearchType>(y => y == searchType),
            It.IsAny<PostCountParams>())
        )
            .ReturnsAsync(ServiceResponse.CreateOk(new object()));

        // Act
        IActionResult result = await _controller.CountAsync(searchType, new());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [MemberData(nameof(RequestsWithValidTags))]
    public async Task Create_Ok(EditRequest request)
    {
        // Setup
        _mockService.Setup(x => x.CreateAsync(It.IsAny<EditRequest>()))
            .ReturnsAsync(ServiceResponse.CreateOk(1));

        // Act
        IActionResult result = await _controller.CreateAsync(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [MemberData(nameof(RequestsWithInvalidTags))]
    public async Task Create_InvalidTags(EditRequest request, string[] expectedFields)
    {
        // Setup
        _mockService.Setup(x => x.CreateAsync(It.IsAny<EditRequest>()))
            .ReturnsAsync(ServiceResponse.CreateOk(1));

        // Act
        IActionResult result = await _controller.CreateAsync(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        ErrorMessages messages = (ErrorMessages)((BadRequestObjectResult)result).Value;
        Assert.Equal(expectedFields, messages.Errors.Keys);

    }

    [Theory]
    [MemberData(nameof(RequestsWithValidTags))]
    public async Task Edit_Ok(EditRequest request)
    {
        // Setup
        _mockService.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<EditRequest>()))
            .ReturnsAsync(ServiceResponse.CreateOk());

        // Act
        IActionResult result = await _controller.EditAsync(1, request);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Theory]
    [MemberData(nameof(RequestsWithInvalidTags))]
    public async Task Edit_InvalidTags(EditRequest request, string[] expectedFields)
    {
        // Setup
        _mockService.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<EditRequest>()))
            .ReturnsAsync(ServiceResponse.CreateOk());

        // Act
        IActionResult result = await _controller.EditAsync(1, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        ErrorMessages messages = (ErrorMessages)((BadRequestObjectResult)result).Value;
        Assert.Equal(expectedFields, messages.Errors.Keys);

    }

    public static IEnumerable<object[]> RequestsWithValidTags => new List<object[]>
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
    };

    public static IEnumerable<object[]> RequestsWithInvalidTags => new List<object[]>
    {
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag
                    {
                        Id = null
                    }
                }
            },
            new[] { "Tags[0]" }
        },
        new object[]
        {
            new EditRequest
            {
                Tags = new[]
                {
                    new TopicTag(),
                    new TopicTag
                    {
                        Name = "Fancy"
                    },
                    new TopicTag(),
                }
            },
            new[] { "Tags[0]", "Tags[2]" }
        },
    };
}
