using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AirlineSendAgent.App;
using AirlineSendAgent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AirlineSendAgent.Client;
using System.IO; 

namespace AirlineSendAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(typeof(Program).Assembly, optional: true, reloadOnChange: true)
                .AddCommandLine(args);

            var configuration = builder.Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    //services.AddHostedService<FlightDetailService>();
                    services.AddSingleton<IAppHost, AppHost>();
                    services.AddDbContext<SendAgentDbContext>(options =>
                    {
                        //options.UseSqlServer("Server=Desktop-TITA3TC\\VE_SERVER;Database=AirlineWebDB;User Id=sa;Password=Password123!;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true");
                        options.UseSqlServer(context.Configuration.GetConnectionString("AirlineConnection"));
                    });

                    services.AddHttpClient();
                })
                .Build();

            host.Services.GetService<IAppHost>().Run();
        }
    }
}