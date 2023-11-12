using Sonab.Core.Entities;
using Sonab.WebAPI.Extensions;
using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Posts;
using Sonab.WebAPI.Repositories.Abstract;
using Sonab.WebAPI.Services.Abstract;

namespace Sonab.WebAPI.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IHttpContextAccessor _accessor;
    private readonly IPostRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ITopicRepository _topicRepository;

    public PostService(
        ILogger<PostService> logger,
        IHttpContextAccessor accessor,
        IPostRepository repository,
        IUserRepository userRepository,
        ISubscriptionRepository subscriptionRepository,
        ITopicRepository topicRepository)
    {
        _logger = logger;
        _accessor = accessor;
        _repository = repository;
        _userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        _topicRepository = topicRepository;
    }

    public async Task<ServiceResponse> GetListAsync(SearchType search, PostListParams listParams)
    {
        List<PostShortInfo> result = search switch
        {
            SearchType.All => await _repository.GetAsync(listParams),
            SearchType.User => await _repository
                .GetUserPostsAsync(_accessor.GetUserId(), listParams),
            SearchType.Publishers => await _repository
                .GetPublishersPostsAsync(_accessor.GetUserId(), listParams),
            _ => throw new ArgumentException("Invalid search type")
        };
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> CountAsync(SearchType search, PostCountParams countParams)
    {
        int result = search switch
        {
            SearchType.All => await _repository.CountAsync(countParams),
            SearchType.User => await _repository
                .CountUserPostAsync(_accessor.GetUserId(), countParams),
            SearchType.Publishers => await _repository
                .CountPublishersPostAsync(_accessor.GetUserId(), countParams),
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
            Tags = post.Topics.Select(x => new TopicTag
            {
                Id = x.Id,
                Name = x.Name
            }).ToArray(),
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
        string userId = _accessor.GetUserId();
        User user = await _userRepository.GetByExternalIdAsync(userId);
        if (user == null)
        {
            _logger.LogError($"User information is not loaded. ID: '{userId}'");
            return ServiceResponse.CreateConflict(Messages.InfoNotLoaded);
        }

        (List<int> ids, List<string> names) = request.SplitTags();
        List<Topic> topics = new(names.Select(name => new Topic(name)));
        if (ids.Count > 0)
        {
            List<Topic> existingTopics = await _topicRepository.GetAsync(ids);
            ids.RemoveAll(x => existingTopics.Any(y => y.Id == x));
            if (ids.Count > 0)
            {
                string[] fieldNames = request.GetFieldsByIds(ids);
                return ServiceResponse.CreateNotFound(Messages.NotFound, fieldNames);
            }

            topics.AddRange(existingTopics);
        }

        Post post = new(request.Title, request.Content, user, topics);
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

        (List<int> ids, List<string> names) = request.SplitTags();
        List<Topic> topics = new(names.Select(name => new Topic(name)));
        if (ids.Count > 0)
        {
            List<Topic> existingTopics = await _topicRepository.GetAsync(ids);
            ids.RemoveAll(x => existingTopics.Any(y => y.Id == x));
            if (ids.Count > 0)
            {
                string[] fieldNames = request.GetFieldsByIds(ids);
                return ServiceResponse.CreateNotFound(Messages.NotFound, fieldNames);
            }

            topics.AddRange(existingTopics);
        }

        post.Update(request.Title, request.Content, topics);
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
