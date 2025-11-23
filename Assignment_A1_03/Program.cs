using Assignment_A1_03.Models;
using Assignment_A1_03.Services;

namespace Assignment_A1_03;

class Program
{
    static void Main(string[] args)
    {
        OpenWeatherService service = new OpenWeatherService();

        //Register the event
        //Your Code

        Task<Forecast>[] tasks = { null, null, null, null };
        Exception exception = null;
        try
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;

            //Create the two tasks and wait for comletion
            tasks[0] = service.GetForecastAsync(latitude, longitude);
            tasks[1] = service.GetForecastAsync("Miami");

            Task.WaitAll(tasks[0], tasks[1]);

            tasks[2] = service.GetForecastAsync(latitude, longitude);
            tasks[3] = service.GetForecastAsync("Miami");

            //Wait and confirm we get an event showing cahced data avaialable
            Task.WaitAll(tasks[2], tasks[3]);
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

