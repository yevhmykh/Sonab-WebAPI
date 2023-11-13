namespace Sonab.Core.Entities;

public class Topic : AggregateRoot
{
    public string Name { get; protected set; }
    public string NormalizedName { get; protected set; }

    public List<Post> Posts { get; protected set; }

    public Topic(string name)
    {
        Name = name;
        NormalizedName = name.ToUpper();
    }

    protected Topic()
    {
    }
}
