namespace Sonab.WebAPI.Models.DB;

public class Comment : Key
{
    public string Content { get; set; }
    public bool IsEdited { get; set; }
    public DateTime Created { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    
    public Post Post { get; set; }
    public User User { get; set; }
}
