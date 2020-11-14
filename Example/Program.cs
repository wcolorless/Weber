using System;
using TestEnvironment.controllers;
using Weber.Core.Server;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var weber = WeberServer
                    .Init("http://localhost:3000/", new WeberServerSettings()
                    {
                        InitControllersFromAssembly = false
                    })
                    .AddService<ITestService, TestService>()
                    .AddController<TestController>()
                    .Go();
                Console.WriteLine($"Type 'help' to get info about command");
                while (true)
                {
                   var input = Console.ReadLine();
                   switch (input)
                   {
                       case "help":
                        Console.WriteLine("start : Will start server execution");
                        Console.WriteLine("stop  : Will stop server execution");
                        break;
                       case "start":
                        if (weber != null && !weber.IsActive)
                        {
                            weber.Go();
                        } break;
                       case "stop":
                        if (weber != null && weber.IsActive)
                        {
                            weber.Stop();
                        } break;
                   }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Weber Example Main Error: {e.Message}");
            }
        }
    }
}
