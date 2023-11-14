using Sonab.Core.Errors;

namespace Sonab.Core.Entities;

public class User : AggregateRoot
{ 
    public string ExternalId { get; protected set; }
    public string Email { get; protected set; }
    public string Name { get; protected set; }

    public virtual List<Post> Posts { get; protected set; }
    public virtual List<Comment> Comments { get; protected set; }
    public virtual List<Mark> Marks { get; protected set; }
    public virtual List<UserSubscription> Subscriptions { get; protected set; }
    public virtual List<UserSubscription> Subscribers { get; protected set; }

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

    public bool TryAddSubscription(User publisher, out UserError error)
    {
        if (Subscriptions.Any(subscription => subscription.PublisherId == publisher.Id))
        {
            error = UserError.AlreadySubscribed();
            return false;
        }
        
        Subscriptions.Add(new UserSubscription(this, publisher));
        error = null;
        return true;
    }

    public bool TryRemoveSubscription(int publisherId, out UserError error)
    {
        UserSubscription subscription = Subscriptions
            .FirstOrDefault(subscription => subscription.PublisherId == publisherId);
        if (subscription == null)
        {
            error = UserError.NotSubscribed();
            return false;
        }

        Subscriptions.Remove(subscription);
        error = null;
        return true;
    }
}
