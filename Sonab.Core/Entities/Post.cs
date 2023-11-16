using Sonab.Core.Errors;

namespace Sonab.Core.Entities;

public class Post : AggregateRoot
{
    /// <summary>
    /// Article title minimal length
    /// </summary>
    public const int TitleMinLength = 2;

    /// <summary>
    /// Article title maximum length
    /// </summary>
    public const int TitleMaxLength = 100;

    /// <summary>
    /// Article content minimal length
    /// </summary>
    public const int ContentMinLength = 200;
    
    internal static Post TryCreate(string title, string content, User user, List<Topic> topics, out PostError error)
    {
        if (!IsTitleValid(title, out error))
        {
            return null;
        }
        if (!IsContentValid(content, out error))
        {
            error = PostError.MinLenghtViolation(nameof(Content), ContentMinLength);
            return null;
        }

        error = null;
        return new Post(title, content, user, topics);
    }
    
    public string Title { get; protected set; }
    public string Content { get; protected set; }
    public DateTime Created { get; protected set; }
    public int UserId { get; protected set; }
    
    public User User { get; protected set; }
    public List<Topic> Topics { get; protected set; }

    private Post(string title, string content, User user, List<Topic> topics)
    {
        Title = title;
        Content = content;
        User = user;
        Topics = topics;
    }

    protected Post()
    {
    }

    public bool TryUpdate(string title, string content, List<Topic> topics, out PostError error)
    {
        if (!IsTitleValid(title, out error))
        {
            return false;
        }
        Title = title;

        if (!IsContentValid(content, out error))
        {
            error = PostError.MinLenghtViolation(nameof(Content), ContentMinLength);
            return false;
        }
        Content = content;
        
        Topics = topics;

        error = null;
        return true;
    }

    private static bool IsTitleValid(string value, out PostError error)
    {
        if (string.IsNullOrEmpty(value) || value.Length < TitleMinLength)
        {
            error = PostError.MinLenghtViolation(nameof(Title), TitleMinLength);
            return false;
        }
        if (value.Length > TitleMaxLength)
        {
            error = PostError.MaxLenghtViolation(nameof(Title), TitleMaxLength);
            return false;
        }
        error = null;
        return true;
    }

    private static bool IsContentValid(string value, out PostError error)
    {
        if (string.IsNullOrEmpty(value) || value.Length < ContentMinLength)
        {
            error = PostError.MinLenghtViolation(nameof(Content), ContentMinLength);
            return false;
        }
        error = null;
        return true;
    }
}
