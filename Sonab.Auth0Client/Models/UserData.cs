using System.Text.Json.Serialization;

namespace Sonab.Auth0Client.Models;

public class UserData
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
