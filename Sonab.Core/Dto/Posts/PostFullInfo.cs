namespace Sonab.Core.Dto.Posts;

public class PostFullInfo : PostShortInfo
{
    public bool IsEditAllowed { get; set; }
    public bool IsSubscribed { get; set; }
}
