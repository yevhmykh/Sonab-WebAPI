using System.ComponentModel.DataAnnotations;
using Sonab.Core.Constants;

namespace Sonab.Core.Dto.Posts;

public class TopicTag
{
    public int? Id { get; set; }
    [MinLength(Limits.NameMinLength)]
    public string Name { get; set; }
}
