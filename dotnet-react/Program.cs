using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace dotnet_react
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("FUNCTIONS_HTTPWORKER_PORT") ?? "5000";

                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls($"http://+:{port}");
                    
                    var root = Environment.GetEnvironmentVariable("ServerSubfolder");
                    if (!string.IsNullOrEmpty(root))
                    {
                        var currentDirectory = Directory.GetCurrentDirectory();
                        webBuilder.UseContentRoot(Path.Combine(currentDirectory, root));
                    }
                });
    }
}
