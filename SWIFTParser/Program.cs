using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerServiceApp1.Services;

namespace WorkerServiceApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            CreateDatabaseIfNotExist(host);

            host.Run();
        }

        private static void CreateDatabaseIfNotExist(IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SWIFTParserDBContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AppSettings.Configuration = configuration;
                    AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConn");

                    var optionBuilder = new DbContextOptionsBuilder<SWIFTParserDBContext>();
                    optionBuilder.UseSqlServer(AppSettings.ConnectionString);

                    services.AddScoped<SWIFTParserDBContext>(d => new SWIFTParserDBContext(optionBuilder.Options));

                    services.AddHostedService<Worker>();
                });
    }
}
