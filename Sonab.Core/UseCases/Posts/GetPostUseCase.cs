using Sonab.Core.Dto;
using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Responses;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts;

public class GetPostUseCase : IUseCase<GetPostRequest, GetPostResponse>
{
    private readonly IPostRepository _repository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public GetPostUseCase(IPostRepository repository, ISubscriptionRepository subscriptionRepository)
    {
        _repository = repository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task Handle(
        LoggedInUser loggedInUser,
        GetPostRequest request,
        IPresenter<GetPostResponse> presenter)
    {
        Post post = await _repository.GetFullInfoAsync(request.PostId);
        
        
        string userId = loggedInUser?.ExternalId;
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
            IsEditAllowed = post.User.ExternalId == userId // TODO: Should be split to another use case?
        };
        if (!result.IsEditAllowed && userId != null)
        {
            result.IsSubscribed = await _subscriptionRepository
                .IsSubscribedAsync(post.User, userId); // TODO: Same?
        }
        await presenter.HandleSuccess(new GetPostResponse(result));
    }
}
