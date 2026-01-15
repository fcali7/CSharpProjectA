using Assignment_A1_02.Models;
using Newtonsoft.Json;

namespace Assignment_A1_02.Services;

public class OpenWeatherService
{
    readonly HttpClient _httpClient = new HttpClient();
    readonly string _apiKey = "3c7df5a7e714caaba0dc9513070db525"; // Replace with your OpenWeatherMap API key

    //Event declaration
    public event EventHandler<string> WeatherForecastAvailable;

    protected virtual void OnWeatherForecastAvailable(string message)
    {
        WeatherForecastAvailable?.Invoke(this, message);
    }

    public async Task<Forecast> GetForecastAsync(string City)
    {
        //////https://openweathermap.org/current
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={_apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        OnWeatherForecastAvailable($"New weather forecast for {forecast.City} available");

        return forecast;
    }

    public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
    {
        //https://openweathermap.org/current
        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={_apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);

        OnWeatherForecastAvailable($"New weather forecast for {forecast.City} available");

        return forecast;
    }

    private async Task<Forecast> ReadWebApiAsync(string uri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        WeatherApiData wd = JsonConvert.DeserializeObject<WeatherApiData>(content);

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