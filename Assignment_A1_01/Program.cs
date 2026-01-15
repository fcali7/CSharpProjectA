/*
 * Inlämning: Project Part A
 * Namn: Ali Ahmadi
 * GitHub Repo: https://github.com/fcali7/CSharpProjectA.git
 */

using Assignment_A1_01.Models;
using Assignment_A1_01.Services;

namespace Assignment_A1_01;

class Program
{
    static async Task Main(string[] args)
    {
        double latitude = 59.5086798659495;
        double longitude = 18.2654625932976;

        try
        {
            Forecast forecast = await new OpenWeatherService().GetForecastAsync(latitude, longitude);

            Console.WriteLine($"Weather forecast for {forecast.City}");
            Console.WriteLine("----------------------------------");

            
            var dailyGroups = forecast.Items.GroupBy(item => item.DateTime.Date);

            foreach (var group in dailyGroups)
            {
                
                Console.WriteLine(group.Key.ToString("yyyy-MM-dd"));

                foreach (var item in group)
                {
                    
                    Console.WriteLine($"   {item.DateTime:HH:mm}: {item.Description}, Temperature: {item.Temperature} degree C, Wind: {item.WindSpeed} m/s");
                }
                Console.WriteLine(); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Något gick fel (Har du lagt in API-nyckeln?): {ex.Message}");
        }
    }
}

