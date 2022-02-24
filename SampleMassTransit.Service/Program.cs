﻿namespace SampleMassTransit.Service
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Components;

    using MassTransit;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Serilog;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var rabbitMqOption = hostContext.Configuration
                        .GetSection("RabbitMqOption")
                        .Get<RabbitMqOption>();
                    
                    services.AddMassTransit(cfg =>
                    { 
                        cfg.SetKebabCaseEndpointNameFormatter();
                        cfg.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();
                        cfg.UsingRabbitMq((context, config) =>
                        {
                            config.Host(rabbitMqOption.HostAddress, 
                                rabbitMqOption.Port, 
                                rabbitMqOption.VirtualHost, c =>
                            {
                                c.Username(rabbitMqOption.Username);
                                c.Password(rabbitMqOption.Password);
                            });

                            config.ConfigureEndpoints(context);
                        });
                    });

                    services.AddMassTransitHostedService();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddSerilog(dispose: true);
                    logging.AddConsole();
                });

            if (isService)
                await builder.UseWindowsService().Build().RunAsync();
            else
                await builder.RunConsoleAsync();
        }
    }
}