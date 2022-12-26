using Sonab.WebAPI.Extentions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;
using Sonab.WebAPI.Utils.Constants;

namespace Sonab.WebAPI.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IHttpContextAccessor _accessor;
    private readonly IPostRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public PostService(
        ILogger<PostService> logger,
        IHttpContextAccessor accessor,
        IPostRepository repository,
        IUserRepository userRepository,
        ISubscriptionRepository subscriptionRepository)
    {
        _logger = logger;
        _accessor = accessor;
        _repository = repository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<ServiceResponse> GetListAsync(SearchType search, ListParams listParams)
    {
        PostShortInfo[] result = search switch  {
            SearchType.All => await _repository.GetAsync(listParams),
            SearchType.User => await _repository
                .GetUserPostsAsync(_accessor.GetUserId(), listParams),
            SearchType.Publishers => await _repository
                .GetPublishersPostsAsync(_accessor.GetUserId(), listParams),
            _ => throw new ArgumentException("Invalid search type")
        };
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> CountAsync(SearchType search)
    {
        int result = search switch  {
            SearchType.All => await _repository.CountAsync(),
            SearchType.User => await _repository
                .CountUserPostAsync(_accessor.GetUserId()),
            SearchType.Publishers => await _repository
                .CountPublishersPostAsync(_accessor.GetUserId()),
            _ => throw new ArgumentException("Invalid search type")
        };
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> GetByIdAsync(int id)
    {
        Post post = await _repository.GetFullInfoAsync(id);
        if (post == null)
        {
            return ServiceResponse.CreateNotFound();
        }

        PostFullInfo result = new()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.UserId,
            Author = post.User.Name,
            IsEditAllowed = _accessor.TryGetUserId(out string userId) &&
                post.User.ExternalId == userId
        };
        if (!result.IsEditAllowed && userId != null)
        {
            result.IsSubscribed = await _subscriptionRepository
                .IsSubscribedAsync(post.User, userId);
        }

        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> CreateAsync(EditRequest request)
    {
        User user = await _userRepository.GetByExternalIdAsync(_accessor.GetUserId());
        if (user == null)
        {
            _logger.LogError($"User information is not loaded. ID: '{_accessor.GetUserId()}'");
            return ServiceResponse.CreateConflict(Messages.InfoNotLoaded);
        }

        Post post = new()
        {
            Title = request.Title,
            Content = request.Content,
            User = user
        };
        await _repository.AddAndSaveAsync(post);

        return ServiceResponse.CreateOk(post.Id);
    }

    public async Task<ServiceResponse> UpdateAsync(int id, EditRequest request)
    {
        Post post = await _repository.GetFullInfoAsync(id);
        if (post == null)
        {
            return ServiceResponse.CreateNotFound();
        }
        if (!post.User.ExternalId.Equals(_accessor.GetUserId(), StringComparison.OrdinalIgnoreCase))
        {
            return ServiceResponse.CreateForbidden(Messages.OnlyOwner);
        }

        post.Content = request.Content;
        post.Title = request.Title;
        await _repository.UpdateAndSaveAsync(post);

        return ServiceResponse.CreateOk();
    }

    public async Task<ServiceResponse> RemoveAsync(int id)
    {
        Post post = await _repository.GetFullInfoAsync(id);
        if (post == null)
        {
            return ServiceResponse.CreateNotFound();
        }
        if (!post.User.ExternalId.Equals(_accessor.GetUserId(), StringComparison.OrdinalIgnoreCase))
        {
            return ServiceResponse.CreateForbidden(Messages.OnlyOwner);
        }

        await _repository.DeleteAndSaveAsync(post);

        return ServiceResponse.CreateOk();
    }
}
