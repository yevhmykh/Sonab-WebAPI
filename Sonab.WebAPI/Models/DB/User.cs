namespace Sonab.WebAPI.Models.DB;

public class User : Key
{
    public string ExternalId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public List<Post> Posts { get; set; }
    public List<UserSubscription> Subscriptions { get; set; }
    public List<UserSubscription> Subscribers { get; set; }
}
