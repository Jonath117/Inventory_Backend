using Microsoft.OpenApi;
using Purchases.Infrastructure;
using Purchases.Application;
using Shared.API.Filters;
using Shared.Application;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPurchaseInfrastructure(builder.Configuration);
builder.Services.AddPurchaseApplication();

builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCoreApplication();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("purchases", new OpenApiInfo { Title = "Modulo de Compras", Version = "v1" }); 
});


builder.Services.AddControllers();
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

builder.Services.AddControllers(options => 
{
    options.Filters.Add<CompanyTenantFilter>();
});

DotNetEnv.Env.Load();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/purchases/swagger.json", "Modulo de Compras");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();