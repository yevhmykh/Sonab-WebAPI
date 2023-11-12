namespace Sonab.Core.Entities;

public class UserSubscription
{
    public int UserId { get; protected set; }
    public int PublisherId { get; protected set; }
    
    public User User { get; protected set; }
    public User Publisher { get; protected set; }

    public UserSubscription(User user, User publisher)
    {
        User = user;
        Publisher = publisher;
    }

    protected UserSubscription()
    {
    }
}
