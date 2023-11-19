using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sonab.Core.Constants;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Dto.Posts.Requests.List;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.Core.UseCases.Posts;
using Sonab.Core.UseCases.Posts.Count;
using Sonab.Core.UseCases.Posts.List;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Presenters.Posts;

namespace Sonab.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("posts")]
public class PostController : BaseController
{
    private readonly ILogger<PostController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPostRepository _postRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITopicRepository _topicRepository;

    public PostController(
        ILogger<PostController> logger,
        IUnitOfWork unitOfWork,
        IPostRepository postRepository,
        ISubscriptionRepository subscriptionRepository, IUserRepository userRepository, ITopicRepository topicRepository)
    {
        _logger = logger;
        _postRepository = postRepository;
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    [AllowAnonymous]
    [HttpGet("{search}/list")]
    [ProducesResponseType(typeof(PostShortInfo[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(
        [FromRoute] PostSearchType search,
        [FromQuery] PostListParams listParams)
    {
        if (search != PostSearchType.All && !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        return await ExecuteUseCase(
            new GetPostListRequest(listParams),
            search switch
            {
                PostSearchType.All => new GetAllPostsUseCase(_postRepository),
                PostSearchType.User => new GetUserPostsUseCase(_postRepository),
                PostSearchType.Publishers => new GetPublishersPostsUseCase(_postRepository),
                _ => throw new ArgumentOutOfRangeException(nameof(search), search, "Invalid search type")
            },
            new PostsPresenter()
        );
    }

    [AllowAnonymous]
    [HttpGet("{search}/count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> CountAsync(
        [FromRoute] PostSearchType search,
        [FromQuery] PostCountParams countParams)
    {
        if (search != PostSearchType.All && !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        return await ExecuteUseCase(
            new GetPostCountRequest(countParams),
            search switch
            {
                PostSearchType.All => new GetAllPostCountUseCase(_postRepository),
                PostSearchType.User => new GetUserPostCountUseCase(_postRepository),
                PostSearchType.Publishers => new GetPublishersPostCountUseCase(_postRepository),
                _ => throw new ArgumentOutOfRangeException(nameof(search), search, "Invalid search type")
            },
            new PostCountPresenter()
        );
    }

    [AllowAnonymous]
    [HttpGet("get/{id:int}")]
    [ProducesResponseType(typeof(PostFullInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        return ExecuteUseCase(
            new GetPostRequest(id),
            new GetPostUseCase(_postRepository, _subscriptionRepository),
            new PostPresenter()
        );
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] EditRequest request)
    {
        if (!request.IsTagsValid(out string[] fieldNames))
        {
            _logger.LogDebug($"Bad tags: {request}");
            return BadRequest(new ErrorMessages(fieldNames, Messages.TagsInvalid));
        }

        (List<int> ids, List<string> names) = request.SplitTags();
        return await ExecuteUseCase(
            new CreatePostRequest(request.Title, request.Content, ids, names),
            new CreatePostUseCase(_unitOfWork),
            new CreatePostPresenter(request)
        );
    }

    [HttpPut("edit/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] EditRequest request)
    {
        if (!request.IsTagsValid(out string[] fieldNames))
        {
            _logger.LogDebug($"Bad tags: {request}");
            return BadRequest(new ErrorMessages(fieldNames, Messages.TagsInvalid));
        }

        (List<int> ids, List<string> names) = request.SplitTags();
        return await ExecuteUseCase(
            new EditPostRequest(id, request.Title, request.Content, ids, names),
            new EditPostUseCase(_unitOfWork),
            new EditPostPresenter(request)
        );
    }

    [HttpDelete("remove/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> RemoveAsync([FromRoute] int id)
    {
        return ExecuteUseCase(
            new RemovePostRequest(id),
            new RemovePostUseCase(_unitOfWork, _userRepository),
            new RemovePostPresenter()
        );
    }
}
