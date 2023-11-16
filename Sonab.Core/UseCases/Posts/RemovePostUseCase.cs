using Sonab.Core.Dto;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Entities;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts;

public class RemovePostUseCase : AuthorizedUseCase<RemovePostRequest, OkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Post> _repository;
    private readonly IUserRepository _userRepository;

    public RemovePostUseCase(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.GetRepository<Post>();
        _userRepository = userRepository;
    }
    
    protected override async Task Handle(string userExternalId, RemovePostRequest request, IPresenter<OkResponse> presenter)
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

        await _repository.DeleteAsync(post);
        await _unitOfWork.Commit();
        await presenter.HandleSuccess(new OkResponse());
    }
}
