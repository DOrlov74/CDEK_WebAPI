using System.Text.Json.Serialization;

namespace CDEK_WebAPI.Model
{
  public class Tarif:IDataRequest
  {
    [JsonPropertyName("type")]
    public int Order_type { get; set; } = 2;
    [JsonPropertyName("currency")]
    public int Currency { get; set; } = 1;
    [JsonPropertyName("tariff_code")]
    public int Tariff_code { get; set; } = 121;
    [JsonPropertyName("from_location")]
    public Location? From_location { get; set; }
    [JsonPropertyName("to_location")]
    public Location? To_location { get; set; }
    [JsonPropertyName("packages")]
    public List<Package> Packages { get; set; } = new ();
  }
}
