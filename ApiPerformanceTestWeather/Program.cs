using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiPerformanceTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            int numberOfRequests = 100;
            string targetUrl = "http://192.168.0.10:7269/Weather/Dublin";

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < numberOfRequests; i++)
            {
                var response = await httpClient.GetAsync(targetUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Request {i} failed: {response.StatusCode}");
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"Total time for {numberOfRequests} requests: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}