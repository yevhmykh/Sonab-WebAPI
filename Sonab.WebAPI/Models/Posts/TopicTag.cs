using System.ComponentModel.DataAnnotations;

namespace Sonab.WebAPI.Models.Posts;

public class TopicTag
{
    public int? Id { get; set; }
    [MinLength(Limits.NameMinLength)]
    public string Name { get; set; }
}
