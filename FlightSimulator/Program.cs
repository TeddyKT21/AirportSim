using ConsoleSimulator.Models;
using System.Net.Http.Json;

namespace FlightSimulator

{
    internal class Program
    {
         static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5001") };
        static void Main(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            timer.Elapsed += (s, e) => CreateFlight();
            timer.Elapsed += (s, e) => ChangeTimerInterval(s!);
            timer.Start();
            Console.WriteLine("simulator started");
            Console.ReadKey();
        }


        static async void CreateFlight()
        {
            var flight = new FlightDto();
            Console.WriteLine($"Flight sent:");
            Console.WriteLine($"{flight}");
            var response = await client.PostAsJsonAsync("api/Flights", flight);
            Console.WriteLine("Respnse:");
            Console.WriteLine(response);
        }

        public static void ChangeTimerInterval(object source)
        {
            var timer = source as System.Timers.Timer;
            Random rnd = new Random();
            timer!.Interval = rnd.Next(12000, 30000);
        }
    }
}