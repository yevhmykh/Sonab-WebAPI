namespace Sonab.Core.Entities;

public class Mark : Entity
{
    public int Value { get; protected set; }
    public int PostId { get; protected set; }
    public int UserId { get; protected set; }
    
    public Post Post { get; protected set; }
    public User User { get; protected set; }
    
    // Planned feature
}
