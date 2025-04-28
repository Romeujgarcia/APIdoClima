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

        // Verifica a conexão com o Redis
        try
        {
            // Tenta obter um valor simples para verificar a conexão
            var testKey = "test_connection";
            _cache.GetString(testKey); // Isso não deve lançar exceção se a conexão estiver ok
        }
        catch (Exception ex)
        {
            // Log de erro ou tratamento apropriado
            Console.WriteLine($"Erro ao conectar ao Redis: {ex.Message}");
        }
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        var apiKey = _configuration["AppSettings:APIKey"];
        var cacheKey = $"WeatherData:{city}";

        try
        {
            // Tenta obter dados do cache
            var cachedWeatherData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedWeatherData))
            {
                return Ok(cachedWeatherData);
            }

            // Faz a chamada para a API de clima
            var response = await _httpClient.GetAsync($"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{city}?unitGroup=us&key={apiKey}&contentType=json");

            if (!response.IsSuccessStatusCode)
            {
                // Retorna uma mensagem de erro apropriada se a API de terceiros falhar
                return StatusCode((int)response.StatusCode, $"Erro ao obter dados do clima: {response.ReasonPhrase}");
            }

            var weatherData = await response.Content.ReadAsStringAsync();

            // Armazena os dados no cache com uma expiração de 5 minutos
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, weatherData, options);

            return Ok(weatherData);
        }
        catch (HttpRequestException httpEx)
        {
            // Tratamento de erro para problemas de rede
            return StatusCode(503, $"Erro de rede ao acessar a API de clima: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            // Tratamento de erro genérico
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}    