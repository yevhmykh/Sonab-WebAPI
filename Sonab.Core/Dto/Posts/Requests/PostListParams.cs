namespace Sonab.Core.Dto.Posts.Requests;

public sealed class PostListParams : ListParams
{
    public int? AuthorId { get; set; }
    public int? TopicTagId { get; set; }
}
