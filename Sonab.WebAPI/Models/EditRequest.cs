using System.ComponentModel.DataAnnotations;
using Sonab.Core.Dto.TopicTags;
using Sonab.Core.Entities;

namespace Sonab.WebAPI.Models;

public class EditRequest
{
    [Required]
    [MinLength(Post.TitleMinLength)]
    [MaxLength(Post.TitleMaxLength)]
    public string Title { get; set; }
    [Required]
    [MinLength(Post.ContentMinLength)]
    public string Content { get; set; }
    public TopicTag[] Tags { get; set; }

    public bool IsTagsValid(out string[] fieldNames)
    {
        TopicTag[] invalid = Tags?
            .Where(x => !x.Id.HasValue && string.IsNullOrWhiteSpace(x.Name))
            .ToArray();

        if (invalid?.Length > 0)
        {
            fieldNames = invalid.Select(x => $"Tags[{Array.IndexOf(Tags, x)}]").ToArray();
            return false;
        }

        fieldNames = null;
        return true;
    }

    public (List<int> ids, List<string> names) SplitTags()
    {
        List<int> ids = new();
        List<string> names = new();

        if (Tags == null)
        {
            return (ids, names);
        }

        foreach (TopicTag tag in Tags)
        {
            if (tag.Id.HasValue)
            {
                ids.Add(tag.Id.Value);
            }
            else
            {
                names.Add(tag.Name);
            }
        }

        return (ids, names);
    }

    public string[] GetFieldsByIds(IEnumerable<int> ids)
    {
        TopicTag[] tags = Tags
            .Where(x => x.Id.HasValue && ids.Contains(x.Id.Value))
            .ToArray();

        return tags.Select(x => $"Tags[{Array.IndexOf(Tags, x)}].Id").ToArray();
    }

    public override string ToString() => Tags?.Length > 0 ?
        $"'{Title}': '{string.Join("','", Tags.Select(x => $"{x.Id}|{x.Name}"))}'" :
        $"'{Title}', no tags";
}
