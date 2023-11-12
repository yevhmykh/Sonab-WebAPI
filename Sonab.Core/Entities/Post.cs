namespace Sonab.Core.Entities;

public class Post : Key
{
    public string Title { get; protected set; }
    public string Content { get; protected set; }
    public DateTime Created { get; protected set; }
    public int UserId { get; protected set; }
    
    public User User { get; protected set; }
    public List<Topic> Topics { get; protected set; }

    public Post(string title, string content, User user, List<Topic> topics)
    {
        Title = title;
        Content = content;
        User = user;
        Topics = topics;
    }

    protected Post()
    {
    }

    public void Update(string title, string content, List<Topic> topics)
    {
        Title = title;
        Content = content;
        Topics = topics;
    }
}
