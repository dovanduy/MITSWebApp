using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MITSDataLib.Seeds;

namespace MITSWebServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            SeedDb(host);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseStartup<Startup>()
            .Build();

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((context, config) =>
        //        {
        //            config
        //                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true);
        //            context.Configuration = config.Build();
        //        })
        //        .ConfigureAppConfiguration(SetupConfiguration)
        //        .UseStartup<Startup>();

        private static void SeedDb(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<MITSSeeder>();
                seeder.SeedAsync().Wait();
            }
        }

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Removing the default configuration options
            builder.Sources.Clear();

            builder.AddJsonFile("config.json", false, true)
                    .AddEnvironmentVariables();

                //TODO: Remove for production
                builder.AddUserSecrets<Startup>();
            

        }
    }
}
