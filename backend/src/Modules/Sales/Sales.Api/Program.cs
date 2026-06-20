using Microsoft.OpenApi;
using Sales.Api.Middlewares;
using Sales.Application;
using Sales.Infrastructure;
using Shared.API.Filters;
using Shared.Application;
using Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSalesInfrastructure(builder.Configuration);
builder.Services.AddSalesApplication();

builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCoreApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("sales", new OpenApiInfo { Title = "Modulo de Ventas", Version = "v1" });
});


builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services.AddControllers(options => 
{
    options.Filters.Add<CompanyTenantFilter>();
});

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/sales/swagger.json", "Modulo de Ventas");
});

//app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var coreDb = scope.ServiceProvider.GetRequiredService<Shared.Infrastructure.CoreDbContext>();
    coreDb.Database.Migrate();

    var salesDb = scope.ServiceProvider.GetRequiredService<Sales.Infrastructure.Persistence.SalesDbContext>();
    salesDb.Database.Migrate();
}

app.Run();