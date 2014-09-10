using System;
using Microsoft.Owin.Hosting;

namespace Sample.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:1337");
            Console.ReadKey();
        }
    }
}