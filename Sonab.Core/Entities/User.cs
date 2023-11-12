namespace Sonab.Core.Entities;

public class User : Key
{ 
    public string ExternalId { get; protected set; }
    public string Email { get; protected set; }
    public string Name { get; protected set; }

    public List<Post> Posts { get; protected set; }
    public List<Comment> Comments { get; protected set; }
    public List<Mark> Marks { get; protected set; }
    public List<UserSubscription> Subscriptions { get; protected set; }
    public List<UserSubscription> Subscribers { get; protected set; }

    public User(string externalId, string email, string name)
    {
        ExternalId = externalId.ToUpper();
        Email = email.ToUpper();
        Name = name;
    }

    protected User()
    {
    }
    
    // Maybe split into 2 methods, because email is identifier too... Or rename to UpdateIdAndName... need better name...
    public void UpdateIdentifiers(string externalId, string name)
    {
        // ID still can be updated, because external "auth" service can send different user identifiers by one email
        ExternalId = externalId.ToUpper();
        Name = name;
    }
}
