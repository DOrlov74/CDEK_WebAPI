using System.Text.Json.Serialization;

namespace CDEK_WebAPI.Model
{
  public class TokenResponse
  {
    [JsonPropertyName("access_token")]
    public string? Token { get; set; }
    [JsonPropertyName("token_type")]
    public string? Type { get; set; }
    [JsonPropertyName("expires_in")]
    public int Expires_in { get; set; }
  }
}
