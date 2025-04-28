using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

public class WeatherController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public WeatherController(HttpClient httpClient, IDistributedCache cache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _configuration = configuration;
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        var apiKey = _configuration["AppSettings:APIKey"];

        var cacheKey = $"WeatherData:{city}";
        
        var cachedWeatherData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedWeatherData))
        {
            return Ok(cachedWeatherData);
        }

        var response = await _httpClient.GetAsync($"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{city}?unitGroup=us&key={apiKey}&contentType=json");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        var weatherData = await response.Content.ReadAsStringAsync();

        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        await _cache.SetStringAsync(cacheKey, weatherData, options);

        return Ok(weatherData);
    }
}