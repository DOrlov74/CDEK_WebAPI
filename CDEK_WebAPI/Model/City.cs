using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CDEK_WebAPI.Model
{
  public class City
  {
    [Key]
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("city")]
    public string? Name { get; set; }
    [JsonPropertyName("fias_guid")]
    public Guid Fias_code { get; set; }
  }
}
