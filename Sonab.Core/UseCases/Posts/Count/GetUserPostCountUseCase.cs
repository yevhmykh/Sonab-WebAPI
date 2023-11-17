using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.Count;

public class GetUserPostCountUseCase : GetPostCountWithAuthorizationUseCase
{
    public GetUserPostCountUseCase(IPostRepository repository) : base(repository)
    {
    }

    protected override Task<int> GetPostCount(string userExternalId, PostCountParams countParams) =>
        Repository.CountUserPostAsync(userExternalId, countParams);
}
