using Assignment_A1_02.Models;
using Assignment_A1_02.Services;

namespace Assignment_A1_02;

class Program
{
    static async Task Main(string[] args)
    {
        OpenWeatherService service = new OpenWeatherService();

        //Register the event
        //Your Code

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
            //Your Code
        }

        foreach (var task in tasks)
        {
            //How to deal with successful and fault tasks
            //Your Code
        }
    }

    //Event handler declaration
    //Your Code
}
