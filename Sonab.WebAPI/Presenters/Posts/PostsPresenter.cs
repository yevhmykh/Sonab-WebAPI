using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Responses;

namespace Sonab.WebAPI.Presenters.Posts;

public class PostsPresenter : ListApiPresenter<GetPostListResponse, PostShortInfo>
{
    protected override List<PostShortInfo> GetItems(GetPostListResponse response) => response.Posts;
}
