using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace Susep.SISRH.WebApi
{
    /// <summary>
    /// Classe de execu��o do programa
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Constr�i o programa inicial
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Cria o Host Web
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Prod")
                   .ConfigureAppConfiguration((hostingContext, config) =>
                       config.SetBasePath(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "Settings"))
                             .AddJsonFile($"connectionstrings.Prod.json", true, true)
                             .AddJsonFile($"appsettings.Prod.json", true, true)
                             .AddJsonFile($"messagebroker.Prod.json", true, true)
                             .AddEnvironmentVariables())
                   .UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration))
                   .UseStartup<Startup>();
    }
}
