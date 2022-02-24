namespace SampleMassTransit.API
{
    using Components;

    using Contracts;

    using MassTransit;
    using MassTransit.Definition;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using MassTransit.Mediator;

    using Microsoft.Extensions.DependencyInjection.Extensions;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*//InMemory
             services.AddMediator(cfg =>
            {
                cfg.AddConsumer<SubmitOrderConsumer>();
                cfg.AddRequestClient<ISubmitOrder>();
            });*/
            
           // services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            
            services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.UsingRabbitMq((context, config) => 
                {
                            
                    var massTransitSection = Configuration.GetSection("RabbitMq");
                    var virtualHost = massTransitSection.GetValue<string>("VirtualHost");
                    var host = massTransitSection.GetValue<string>("HostAddress");
                    var userName = massTransitSection.GetValue<string>("Username");
                    var password = massTransitSection.GetValue<string>("Password");
                    var port = massTransitSection.GetValue<ushort>("Port");
                            
                    config.Host(host, port, virtualHost, c =>
                    {
                        c.Username(userName);
                        c.Password(password);
                    });
                           
                    config.ConfigureEndpoints(context);
                });
                
                cfg.AddRequestClient<ISubmitOrder>();
            });
            
            /*
             * Нжуен при создании шины для ее контроля. Фоновый сервис MassTransit
             */
            services.AddMassTransitHostedService();

            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleMassTransit.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleMassTransit.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}