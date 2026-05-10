using Backend.API.Filters;
using Inventory.Application;
using Inventory.Infrastructure;
using Sales.Application;
using Sales.Infrastructure;
using Shared.Application;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddInventoryApplication();
builder.Services.AddSalesInfrastructure(builder.Configuration);
builder.Services.AddSalesApplication();
builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCoreApplication();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddControllers(options => 
{
    options.Filters.Add<CompanyTenantFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();