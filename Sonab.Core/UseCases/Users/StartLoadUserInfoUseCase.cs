using Sonab.Core.Dto;
using Sonab.Core.Dto.Users;
using Sonab.Core.Interfaces;

namespace Sonab.Core.UseCases.Users;

public class StartLoadUserInfoUseCase : AuthorizedUseCase<StartLoadUserInfoRequest, OkResponse>
{
    protected override Task Handle(
        string userExternalId,
        StartLoadUserInfoRequest request,
        IPresenter<OkResponse> presenter)
    {
        throw new NotImplementedException("Need refactored background worker");
    }
}