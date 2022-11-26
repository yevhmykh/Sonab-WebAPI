namespace Sonab.WebAPI.Models.DB;

public class Topic : Key
{
    public string Name { get; set; }
    
    public List<Post> Posts { get; set; }
}
