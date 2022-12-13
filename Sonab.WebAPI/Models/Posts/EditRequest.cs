using System.ComponentModel.DataAnnotations;
using Sonab.WebAPI.Utils.Constants;

namespace Sonab.WebAPI.Models.Posts;

public class EditRequest
{
    [Required]
    [MinLength(Limits.TitleMinLength)]
    [MaxLength(Limits.TitleMaxLength)]
    public string Title { get; set; }
    [Required]
    [MinLength(Limits.ContentMinLength)]
    public string Content { get; set; }
}
