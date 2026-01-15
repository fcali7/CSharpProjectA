using Assignment_A1_03.Models;
using Assignment_A1_03.Services;

namespace Assignment_A1_03;

class Program
{
    static void Main(string[] args)
    {
        OpenWeatherService service = new OpenWeatherService();

        // Register the event
        service.WeatherForecastAvailable += Service_WeatherForecastAvailable;

        Task<Forecast>[] tasks = { null, null, null, null };
        Exception exception = null;
        try
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;

            // Create the two tasks and wait for completion
            // Första omgången: Dessa ska trigga "New weather forecast..."
            tasks[0] = service.GetForecastAsync(latitude, longitude);
            tasks[1] = service.GetForecastAsync("Miami");

            Task.WaitAll(tasks[0], tasks[1]);

            // Andra omgången (samma anrop direkt efter):
            // Dessa ska trigga "Cached weather forecast..." eftersom det gått mindre än 1 minut
            tasks[2] = service.GetForecastAsync(latitude, longitude);
            tasks[3] = service.GetForecastAsync("Miami");

            // Wait and confirm we get an event showing cached data available
            Task.WaitAll(tasks[2], tasks[3]);
        }
        catch (Exception ex)
        {
            exception = ex;
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Loopa igenom resultaten
        foreach (var task in tasks)
        {
            if (task != null && task.Status == TaskStatus.RanToCompletion)
            {
                Forecast forecast = task.Result;
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
            else if (task?.IsFaulted == true)
            {
                Console.WriteLine($"Task failed: {task.Exception?.InnerException?.Message}");
            }
        }
    }

    // Event handler declaration
    private static void Service_WeatherForecastAvailable(object sender, string message)
    {
        Console.WriteLine($"Event message from weather service: {message}");
    }
}