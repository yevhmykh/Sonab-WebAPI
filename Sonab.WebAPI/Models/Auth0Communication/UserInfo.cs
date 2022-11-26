using System.Text.Json.Serialization;

namespace Sonab.WebAPI.Models.Auth0Communication;

public class UserInfo
{
    [JsonPropertyName("given_name")]
    public string FirstName { get; set; }
    [JsonPropertyName("family_name")]
    public string LastName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("nickname")]
    public string UserName { get; set; }
}
