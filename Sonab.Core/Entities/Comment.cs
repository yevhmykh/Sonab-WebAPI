namespace Sonab.Core.Entities;

public class Comment : Key
{
    public string Content { get; protected set; }
    public bool IsEdited { get; protected set; }
    public DateTime Created { get; protected set; }
    public int PostId { get; protected set; }
    public int UserId { get; protected set; }

    public Post Post { get; protected set; }
    public User User { get; protected set; }
    
    // Planned feature
}
