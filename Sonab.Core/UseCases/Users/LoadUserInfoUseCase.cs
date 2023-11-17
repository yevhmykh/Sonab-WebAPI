using Sonab.Core.Dto;
using Sonab.Core.Dto.Users;
using Sonab.Core.Entities;
using Sonab.Core.Errors;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories;
using Sonab.Core.Interfaces.Services;

namespace Sonab.Core.UseCases.Users;

// TODO: Add some validation to avoid non background worker
public class LoadUserInfoUseCase : IUseCase<LoadUserInfoRequest, OkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IExternalAuthRepository _externalAuthRepository;

    public LoadUserInfoUseCase(IUnitOfWork unitOfWork,
        IExternalAuthRepository externalAuthRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.GetRepository<User>();
        _externalAuthRepository = externalAuthRepository;
    }

    public async Task Handle(LoggedInUser loggedInUser, LoadUserInfoRequest request, IPresenter<OkResponse> presenter)
    {
        UserInfo info = await _externalAuthRepository.GetUserInfoAsync(request.ExternalUserId);
        if (string.IsNullOrEmpty(info.UserName))
        {
            await presenter.HandleFailure(UserError.NoInfo(request.ExternalUserId));
            return;
        }
        
        User user = await _userRepository.GetByAsync(user => user.Email == info.Email.ToUpper());

        if (user == null)
        {
            user = new User(request.ExternalUserId, info.Email, info.UserName);
            await _userRepository.InsertAsync(user);
        }
        else
        {
            user.UpdateIdentifiers(request.ExternalUserId, info.UserName);
            await _userRepository.UpdateAsync(user);
        }

        await _unitOfWork.Commit();
        await presenter.HandleSuccess(new OkResponse());
    }
}
