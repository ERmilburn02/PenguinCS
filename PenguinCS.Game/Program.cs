﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenguinCS.Common;
using PenguinCS.Data;
using Serilog;
using StackExchange.Redis;

namespace PenguinCS.Game;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(null, "appsettings.json", false, true)
            .AddJsonFile(null, $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Starting up the host");

            var builder = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.Configure<PenguinCSOptions>(context.Configuration.GetSection("PenguinCS"));

                    // Redis
                    services.AddSingleton<IConnectionMultiplexer>(sp =>
                    {
                        return ConnectionMultiplexer.Connect(context.Configuration.GetConnectionString("Redis"));
                    });

                    // Postgres
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseNpgsql(context.Configuration.GetConnectionString("Postgres"));
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });

                    // Player Map
                    services.AddSingleton<PlayerMappingService>();

                    // Handlers
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith("PenguinCS"));
                    var handlerTypes = MessageHandlerRegistry.GetHandlerList(assemblies);
                    foreach (var handlerType in handlerTypes)
                    {
                        services.AddTransient(handlerType);
                    }

                    // Registry
                    services.AddSingleton<MessageHandlerRegistry>();
                    services.AddSingleton<MessageProcessor>();

                    // Server
                    services.AddHostedService<GameHostedService>();
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