using System;
using System.Diagnostics;

namespace ServiceHost
{
    class Program
    {
        private static Serilog.ILogger log => Serilog.Log.ForContext<Program>();
        static void Main(string[] args)
        {
            LoggingConfiguration.Configure();
            AuthenticationServiceHost schost = new AuthenticationServiceHost();
            Console.Read();
            Console.WriteLine("Hello World!");
        }
    }
}
