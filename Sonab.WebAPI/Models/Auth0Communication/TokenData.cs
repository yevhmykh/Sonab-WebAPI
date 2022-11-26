using System.Text.Json.Serialization;

namespace Sonab.WebAPI.Models.Auth0Communication;

public class TokenData
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}
