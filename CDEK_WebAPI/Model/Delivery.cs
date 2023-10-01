using System.Text.Json.Serialization;

namespace CDEK_WebAPI.Model
{
  public class Delivery
  {
    [JsonPropertyName("delivery_sum")]
    public float Delivery_sum { get; set; }
  }
}
