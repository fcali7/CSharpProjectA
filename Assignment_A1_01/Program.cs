using Assignment_A1_01.Models;
using Assignment_A1_01.Services;

namespace Assignment_A1_01;

class Program
{
    static async Task Main(string[] args)
    {
        double latitude = 59.5086798659495;
        double longitude = 18.2654625932976;

        Forecast forecast = await new OpenWeatherService().GetForecastAsync(latitude, longitude);

        //Your Code to present each forecast item in a grouped list
        Console.WriteLine($"Weather forecast for {forecast.City}");
    }
}

