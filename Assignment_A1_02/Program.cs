/*
 * Inlämning: Project Part A
 * Namn: Ali Ahmadi
 * GitHub Repo: https://github.com/fcali7/CSharpProjectA.git
 */

using Assignment_A1_02.Models;
using Assignment_A1_02.Services;

namespace Assignment_A1_02;

class Program
{
    static async Task Main(string[] args)
    {
        OpenWeatherService service = new OpenWeatherService();

        //Register the event
        service.WeatherForecastAvailable += Service_WeatherForecastAvailable;

        Task<Forecast>[] tasks = { null, null };
        Exception exception = null;
        try
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;

            //Create the two tasks and wait for comletion
            tasks[0] = service.GetForecastAsync(latitude, longitude);
            tasks[1] = service.GetForecastAsync("Miami");

            await Task.WhenAll(tasks[0], tasks[1]);
        }
        catch (Exception ex)
        {
            exception = ex;
            //How to handle an exception
            Console.WriteLine($"Exception: {ex.Message}");
        }

        foreach (var task in tasks)
        {
            //How to deal with successful and fault tasks
            if (task != null && task.IsCompletedSuccessfully)
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

    //Event handler declaration
    private static void Service_WeatherForecastAvailable(object sender, string message)
    {
        Console.WriteLine($"Event message from weather service: {message}");
    }
}