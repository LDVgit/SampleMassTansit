namespace SampleMassTransit.Infrastructure
{
    using System;
    using System.IO;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class OrderStateDbContextFactory : IDesignTimeDbContextFactory<OrderStateDbContext>
    {
        private readonly IConfiguration _configuration;

        public OrderStateDbContextFactory()
        {
            _configuration = LoadConfiguration();
        }

        public OrderStateDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OrderStateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderStateDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            return new OrderStateDbContext(optionsBuilder.Options);
        }

        private static IConfigurationRoot LoadConfiguration()
        {
            const string connectionJson = "connection.json";
            var projectRootPath = AppContext.BaseDirectory;
            var pathToConnectionJson = Path.Combine(projectRootPath, connectionJson);

            var builder =
                File.Exists(pathToConnectionJson)
                    ? new ConfigurationBuilder()
                        .AddJsonFile(connectionJson, true, true)
                    : new ConfigurationBuilder()
                        .AddJsonFile(Path.Combine(projectRootPath, "appsettings.json"), true, true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}