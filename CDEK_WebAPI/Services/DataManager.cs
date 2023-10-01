using CDEK_WebAPI.Model;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace CDEK_WebAPI.Services
{
  public interface IDataManager
  {
    public Task Auth(string login, string password);
    public Task<City?> GetCity(string fias);
    public Task<City?> GetCityByName(string name);
    public Task<Delivery?> GetDelivery(Tarif tarif);
  }
  public class DataManager : IDataManager
  {
    private string? token ;
    private string baseUrl = "https://api.edu.cdek.ru/v2";

    public async Task Auth(string login, string password)
    {
      if (String.IsNullOrEmpty(token))
      { 
        token = await GetServiceToken(login, password);
        //await FillCities();
      }
    }

    public async Task<City?> GetCity(string fias)
    {
      Guid fiasGuid = new();
      if (String.IsNullOrEmpty(fias) || !TryParseGuid(fias, out fiasGuid))
      {
        return null;
      }
      var citiesUrl = "location/cities";
      NameValueCollection requestParams = new() { { "fias_guid", fiasGuid.ToString() } };
      var response = await SendRequestAsync<List<City>>(HttpMethod.Get, citiesUrl, requestParams);
      if (response != null && response.Count > 0)
      { 
        return response[0];
      }
      return null;
    }

    public async Task<City?> GetCityByName(string name)
    {
      if (String.IsNullOrEmpty(name))
      {
        return null;
      }
      var citiesUrl = "location/cities";
      NameValueCollection requestParams = new() { { "city", name } };
      var response = await SendRequestAsync<List<City>>(HttpMethod.Get, citiesUrl, requestParams);
      if (response != null && response.Count > 0)
      {
        return response[0];
      }
      return null;
    }

    public async Task<Delivery?> GetDelivery(Tarif tarif)
    {
      if (tarif == null)
      {
        return null;
      }
      var tarifUrl = "calculator/tariff";
      return await SendPostRequestAsync<Delivery>(tarifUrl, tarif);
    }

    private async Task<string?> GetServiceToken(string login, string password)
    {
      var authUrl = "oauth/token";
      NameValueCollection requestParams = new()
      {
        { "grant_type", "client_credentials" },
        { "client_id", login },
        { "client_secret", password }
      };
      var response = await SendRequestAsync<TokenResponse>(HttpMethod.Post, authUrl, requestParams);
      return response?.Token;
    }

    private async Task<T?> SendRequestAsync<T>(HttpMethod httpMethod, string entity, NameValueCollection parameters)
    {
      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      var queryString = SerializeParameters(parameters);
      var url = BuildUrl(entity: entity, queryString: queryString);
      using (var httpRequest = CreateHttpRequest(verb: httpMethod, url: url))
      using(var httpResponse = await client.SendAsync(httpRequest))
      {
        httpResponse.EnsureSuccessStatusCode();
        string apiResponse = await httpResponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(apiResponse); 
      }
    }

    private async Task<T?> SendPostRequestAsync<T>(string entity, IDataRequest request)
    {
      var client = new HttpClient();
      var url = BuildUrl(entity: entity);
      using (var httpRequest = CreateHttpRequest(verb: HttpMethod.Post, url: url))
      {
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var options = new JsonSerializerOptions()
        { WriteIndented = true };
        string data = JsonSerializer.Serialize((Tarif)request, options);
        using (var httpContent = new StringContent(data, Encoding.UTF8, "application/json"))
        {
          httpRequest.Content = httpContent;
          using (var httpResponse = await client.SendAsync(httpRequest))
          {
            httpResponse.EnsureSuccessStatusCode();
            string apiResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(apiResponse);
          }
        }
      }
    }

    private HttpRequestMessage CreateHttpRequest(HttpMethod verb, string url)
    {
      var request = new HttpRequestMessage(verb, url);
      if (!string.IsNullOrWhiteSpace(token))
      {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
      }
      return request;
    }

    private string? SerializeParameters(NameValueCollection parameters)
    {
      if (parameters.Count != 0) { 
        List<string> parts = new List<string>();
        foreach (String? key in parameters.AllKeys)
          parts.Add(String.Format("{0}={1}", key, parameters[key]));
        return String.Join("&", parts);
      } else
      {
        return null;
      }
    }

    private string BuildUrl(string entity, string? queryString = null)
    {
      var url = baseUrl;
      if (!String.IsNullOrEmpty(entity))
      {
        url = String.Format("{0}/{1}", url, entity);
      }
      if (queryString != null)
      {
        url += "?" + queryString;
      }
      return url;
    }

    public bool TryParseGuid(string value, out Guid result)
    {
      try
      {
        result = new Guid(value.Replace("-", "")); 
        return true;
      }
      catch
      {
        result = Guid.Empty;
        return false;
      }
    }
  }
}
