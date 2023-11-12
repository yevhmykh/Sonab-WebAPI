namespace Sonab.Core.Entities;

public class Mark : Key
{
    public int Value { get; protected set; }
    public int PostId { get; protected set; }
    public int UserId { get; protected set; }
    
    public Post Post { get; protected set; }
    public User User { get; protected set; }
    
    // Planned feature
}
