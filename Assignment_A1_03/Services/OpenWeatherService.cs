using System.Collections.Concurrent;
using Newtonsoft.Json;
using Assignment_A1_03.Models;

namespace Assignment_A1_03.Services;

public class OpenWeatherService
{
    readonly HttpClient _httpClient = new HttpClient();

    // Cache-listor (Thread Safe)
    readonly ConcurrentDictionary<(double, double, string), Forecast> _cachedGeoForecasts = new ConcurrentDictionary<(double, double, string), Forecast>();
    readonly ConcurrentDictionary<(string, string), Forecast> _cachedCityForecasts = new ConcurrentDictionary<(string, string), Forecast>();

    // Your API Key
    readonly string apiKey = "3c7df5a7e714caaba0dc9513070db525";

    // Event declaration
    public event EventHandler<string> WeatherForecastAvailable;
    protected virtual void OnWeatherForecastAvailable(string message)
    {
        WeatherForecastAvailable?.Invoke(this, message);
    }

    public async Task<Forecast> GetForecastAsync(string City)
    {
        
        string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        var key = (City, dateString);

        
        if (_cachedCityForecasts.TryGetValue(key, out Forecast cachedForecast))
        {
            OnWeatherForecastAvailable($"Cached weather forecast for {City} available");
            return cachedForecast;
        }

        
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        
        _cachedCityForecasts[key] = forecast;

        OnWeatherForecastAvailable($"New weather forecast for {forecast.City} available");

        return forecast;
    }

    public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
    {
        
        string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        var key = (latitude, longitude, dateString);

        if (_cachedGeoForecasts.TryGetValue(key, out Forecast cachedForecast))
        {
            OnWeatherForecastAvailable($"Cached weather forecast for {latitude},{longitude} available");
            return cachedForecast;
        }

        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        _cachedGeoForecasts[key] = forecast;

        OnWeatherForecastAvailable($"New weather forecast for ({latitude},{longitude}) available");

        return forecast;
    }

    private async Task<Forecast> ReadWebApiAsync(string uri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        WeatherApiData wd = JsonConvert.DeserializeObject<WeatherApiData>(content);

        // Convert WeatherApiData to Forecast using Linq
        var forecast = new Forecast
        {
            City = wd.city.name,
            Items = wd.list.Select(item => new ForecastItem
            {
                DateTime = UnixTimeStampToDateTime(item.dt),
                Temperature = item.main.temp,
                WindSpeed = item.wind.speed,
                Description = item.weather.First().description,
                Icon = item.weather.First().icon
            }).ToList()
        };

        return forecast;
    }

    private DateTime UnixTimeStampToDateTime(double unixTimeStamp) => DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
}