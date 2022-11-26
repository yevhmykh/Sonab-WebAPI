namespace Sonab.WebAPI.Models.DB;

public class Post : Key
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public int UserId { get; set; }
    
    public User User { get; set; }
    public List<Topic> Topics { get; set; }
}
