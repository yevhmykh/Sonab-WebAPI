using Sonab.Core.Dto.Posts.Requests;
using Sonab.Core.Dto.Posts.Requests.Count;
using Sonab.Core.Interfaces.Repositories.ReadEntity;

namespace Sonab.Core.UseCases.Posts.Count;

public class GetPublishersPostCountUseCase : GetPostCountWithAuthorizationUseCase<GetPublishersPostCountRequest>
{
    public GetPublishersPostCountUseCase(IPostRepository repository) : base(repository)
    {
    }

    protected override Task<int> GetPostCount(string userExternalId, PostCountParams countParams) =>
        Repository.CountPublishersPostAsync(userExternalId, countParams);
}