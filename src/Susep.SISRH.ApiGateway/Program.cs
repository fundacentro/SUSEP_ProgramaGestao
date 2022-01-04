using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Builder;
using System;

namespace Susep.SISRH.ApiGateway
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Prod")
                    .ConfigureAppConfiguration((hostingContext, config) =>
                       config.SetBasePath(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "Settings"))
                             .AddJsonFile($"appsettings.Prod.json", true, true)
                             .AddJsonFile($"ocelot.Prod.json", true, true)
                             .AddEnvironmentVariables()
                    )
                    .UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration))
                    .UseStartup<Startup>();
    }
}
