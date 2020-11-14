using System;
using System.Collections.Generic;
using System.Text;

namespace TestEnvironment.controllers
{
    public interface ITestService
    {
        string PrintHelloWorld();
    }
    
    public class TestService : ITestService
    {
        public string PrintHelloWorld()
        {
            Console.WriteLine("Hello world");
            return "Hello world";
        }
    }
}
