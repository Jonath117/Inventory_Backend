using Inventory.Api.Middlewares;
using Inventory.Application;
using Inventory.Infrastructure;
using Microsoft.OpenApi;
using Shared.API.Filters;
using Shared.Application;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddInventoryApplication();

builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCoreApplication();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("inventory", new OpenApiInfo { Title = "Modulo de Inventario", Version = "v1" }); 
});

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

builder.Services.AddControllers(options => 
{
    options.Filters.Add<CompanyTenantFilter>();
});


var app = builder.Build();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/inventory/swagger.json", "Modulo de Inventario");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();