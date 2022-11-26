namespace Sonab.WebAPI.Models.DB;

public class UserSubscription
{
    public int UserId { get; set; }
    public int PublisherId { get; set; }
    
    public User User { get; set; }
    public User Publisher { get; set; }
}
