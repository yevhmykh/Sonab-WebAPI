using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Sonab.Core.Dto.Posts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchType
{
    [EnumMember(Value = "all")]
    All = 0,
    [EnumMember(Value = "user")]
    User = 1,
    [EnumMember(Value = "publishers")]
    Publishers = 2
} 
