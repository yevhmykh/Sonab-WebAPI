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

    public PostService(
        ILogger<PostService> logger,
        IHttpContextAccessor accessor,
        IPostRepository repository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _accessor = accessor;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<ServiceResponse> GetListAsync(ListParams listParams)
    {
        PostShortInfo[] result = await _repository.GetAsync(listParams);
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> GetUserPostsAsync(ListParams listParams)
    {
        PostShortInfo[] result = await _repository
            .GetUserPostsAsync(_accessor.GetUserId(), listParams);
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> CountAsync()
    {
        int result = await _repository.CountAsync();
        return ServiceResponse.CreateOk(result);
    }

    public async Task<ServiceResponse> CountUserPostAsync()
    {
        int result = await _repository.CountUserPostAsync(_accessor.GetUserId());
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
            Author = post.User.Name,
            IsEditAllowed = _accessor.TryGetUserId(out string userId) &&
                post.User.ExternalId == userId
        };
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
