using Sonab.Core.Dto;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Entities;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts;

public class EditPostUseCase : AuthorizedUseCase<EditPostRequest, OkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Post> _repository;
    private readonly IUserRepository _userRepository;
    private readonly ITopicRepository _topicRepository;

    public EditPostUseCase(IUnitOfWork unitOfWork, IUserRepository userRepository, ITopicRepository topicRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.GetRepository<Post>();
        _userRepository = userRepository;
        _topicRepository = topicRepository;
    }
    
    protected override async Task Handle(string userExternalId, EditPostRequest request, IPresenter<OkResponse> presenter)
    {
        User user = await _userRepository.GetByExternalIdAsync(userExternalId);

        Post post = await _repository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            await presenter.HandleFailure(PostError.NotFound(request.PostId));
            return;
        }
        if (post.UserId != user.Id)
        {
            await presenter.HandleFailure(PostError.NotOwner());
            return;
        }
        
        List<Topic> topics = new(request.NewTopicTags.Select(name => new Topic(name)));
        if (request.NewTopicTags?.Count > 0)
        {
            List<Topic> existingTopics = await _topicRepository.GetAsync(request.TopicTagIds);
            List<int> missingTopics = request.TopicTagIds.Except(existingTopics.Select(topic => topic.Id)).ToList();
            if (missingTopics.Count > 0)
            {
                await presenter.HandleFailure(PostError.NotFoundTopics(missingTopics));
                return;
            }

            topics.AddRange(existingTopics);
        }

        if (!post.TryUpdate(request.Title, request.Content, topics, out PostError error))
        {
            await presenter.HandleFailure(error);
            return;
        }

        await _repository.UpdateAsync(post);
        await _unitOfWork.Commit();
        await presenter.HandleSuccess(new OkResponse());
    }
}
