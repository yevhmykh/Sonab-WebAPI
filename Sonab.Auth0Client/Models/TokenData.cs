using System.Text.Json.Serialization;

namespace Sonab.Auth0Client.Models;

public class TokenData
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}
