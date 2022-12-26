namespace Sonab.WebAPI.Models.Posts;

public class PostFullInfo : PostShortInfo
{
    public int AuthorId { get; set; }
    public bool IsEditAllowed { get; set; } 
    public bool IsSubscribed { get; set;}
}
