using System.Text.Json.Serialization;

namespace CDEK_WebAPI.Model
{
  public class Location
  {
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("city")]
    public string? Name { get; set; }
  }
}
