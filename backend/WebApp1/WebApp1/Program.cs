using Microsoft.EntityFrameworkCore;
using WebApp1.Business.Services;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


// 1. Conexión a Base de Datos (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
    );

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IAdjustmentService, AdjustmentService>();
builder.Services.AddScoped<IGetStockService, GetStockService>();
builder.Services.AddScoped<ILookUpService, LookUpService>();
builder.Services.AddScoped<IGetProductKardexService, GetProductKardexService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");


app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapGet("/api/test-db", async (ApplicationDbContext context) =>
{
    try
    {
        bool canConnect =  await context.Database.CanConnectAsync();

        if (!canConnect)
        {
            return Results.Problem("Could not connect to database");
        }
        
        var company = await context.Companies.FirstOrDefaultAsync();
        if (company != null)
        {
            return Results.Ok(new
            {
                Mensaje = "Coneccion exitosa",
                EmpresaEncontrada = company.Name
            });
        }
        return Results.Ok("Coneccion exitosa, tablas vacias");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al conectar {ex.Message}");
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}