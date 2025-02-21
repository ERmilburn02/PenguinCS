using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenguinCS.Common;
using PenguinCS.Common.Handlers;
using PenguinCS.Data;
using PenguinCS.Login.Handlers;
using Serilog;
using StackExchange.Redis;

namespace PenguinCS.Login;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(null, "appsettings.json", false, true)
            .AddJsonFile(null, $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Starting up the host");

            var builder = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) => {
                    services.Configure<PenguinCSOptions>(context.Configuration.GetSection("PenguinCS"));

                    // Redis
                    services.AddSingleton<IConnectionMultiplexer>(sp => {
                        return ConnectionMultiplexer.Connect(context.Configuration.GetConnectionString("Redis"));
                    });

                    // Postgres
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("Postgres")));

                    // Handlers
                    services.AddTransient<PolicyHandler>();
                    services.AddTransient<VersionCheckHandler>();
                    services.AddTransient<RandomKeyHandler>();

                    services.AddTransient<LoginHandler>();

                    // Factory
                    services.AddSingleton<MessageHandlerFactory>();

                    // Server
                    services.AddHostedService<LoginHostedService>();
                });

            await builder.Build().RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}