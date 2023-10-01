using CDEK_WebAPI.Model;
using CDEK_WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CDEK_WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DeliveryController : ControllerBase
  {
    private IDataManager _dataManager;
        public DeliveryController(IDataManager dataManager)
        {
            _dataManager = dataManager;
      
    }
    [HttpGet]
    public async Task<ActionResult<string>> GetDelivery([FromQuery]string weight, string height, string length, string width, string from, string to) 
    {
      int weightNum = int.Parse(weight);
      int heightNum = int.Parse(height);
      int lengthNum = int.Parse(length);
      int widthNum = int.Parse(width);
      if (weightNum <= 0 || heightNum <= 0 || lengthNum <= 0 || widthNum <= 0 || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
      {
        return BadRequest();
      }
      await _dataManager.Auth("EMscd6r9JnFiQ3bLoyjJY6eM78JrJceI", "PjLZkKBHEiLK3YsjtNrt3TGNG0ahs3kG");
      City? fromCity = await _dataManager.GetCityByName(from);
      City? toCity = await _dataManager.GetCityByName(to);
      if (fromCity == null || toCity == null) 
      {
        return NotFound();
      }
      Package package = new() { Weight = weightNum, Height=heightNum/10, Length=lengthNum/10, Width=widthNum/10};
      List<Package> packages = new();
      packages.Add(package);
      Location fromLocation = new() { Code=fromCity.Code, Name=fromCity.Name };
      Location toLocation = new() { Code = toCity.Code, Name = toCity.Name };
      Tarif tarif = new() { Packages=packages, From_location=fromLocation, To_location=toLocation };
      Delivery? result = await _dataManager.GetDelivery(tarif);
      return Ok(JsonSerializer.Serialize(result));
    }
  }
}
