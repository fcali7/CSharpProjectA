using System.Collections.Concurrent;
using Newtonsoft.Json;

using Assignment_A1_03.Models;

namespace Assignment_A1_03.Services;

public class OpenWeatherService
{
    readonly HttpClient _httpClient = new HttpClient();
    readonly ConcurrentDictionary<(double, double, string), Forecast> _cachedGeoForecasts = new ConcurrentDictionary<(double, double, string), Forecast>();
    readonly ConcurrentDictionary<(string, string), Forecast> _cachedCityForecasts = new ConcurrentDictionary<(string, string), Forecast>();

    // Your API Key
    readonly string apiKey = "your_api_key_here"; // Replace with your OpenWeatherMap API key

    //Event declaration
    public event EventHandler<string> WeatherForecastAvailable;
    protected virtual void OnWeatherForecastAvailable (string message)
    {
        WeatherForecastAvailable?.Invoke(this, message);
    }
    public async Task<Forecast> GetForecastAsync(string City)
    {
        //part of cache code here to check if forecast in Cache
        //generate an event that shows forecast was from cache
        //Your code
        
        //https://openweathermap.org/current
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        //part of event and cache code here
        //generate an event with different message if cached data
        //Your code

        return forecast;

    }
    public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
    {
        //part of cache code here to check if forecast in Cache
        //generate an event that shows forecast was from cache
        //Your code

        //https://openweathermap.org/current
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        //part of event and cache code here
        //generate an event with different message if cached data
        //Your code

        return forecast;
    }
    private async Task<Forecast> ReadWebApiAsync(string uri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        //Convert Json to NewsResponse
        string content = await response.Content.ReadAsStringAsync();
        WeatherApiData wd = JsonConvert.DeserializeObject<WeatherApiData>(content);

        //Convert WeatherApiData to Forecast using Linq.
        //Your code
        var forecast = new Forecast(); //dummy to compile, replaced by your own code
        return forecast;
    }

    private DateTime UnixTimeStampToDateTime(double unixTimeStamp) => DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
}

