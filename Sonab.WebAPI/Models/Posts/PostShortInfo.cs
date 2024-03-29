namespace Sonab.WebAPI.Models.Posts;

public class PostShortInfo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public string Author { get; set; }
    public TopicTag[] Tags { get; set; }
}
