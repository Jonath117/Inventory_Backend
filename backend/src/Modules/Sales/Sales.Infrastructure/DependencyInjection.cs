using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Sales.Application.Interfaces;
using Sales.Application.Interfaces.ExternalServices;
using Sales.Infrastructure.ExternalServices;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;
using System;
using System.Net.Http;

namespace Sales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "sales"))
                .UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<ISalesRepository, SalesRepository>();
        
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        services.AddHttpClient<IInventoryHttpClient, InventoryHttpClient>(client =>
        {
            var inventoryUrl = configuration["INVENTORY_API_URL"];
            if (inventoryUrl != null) client.BaseAddress = new Uri(inventoryUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(retryPolicy)
        .AddPolicyHandler(circuitBreakerPolicy);

        return services;
    }
}