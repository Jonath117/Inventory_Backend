using Backend.API.Filters;
using Inventory.Application;
using Inventory.Infrastructure;
using Microsoft.OpenApi;
using Purchases.Infrastructure;
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

builder.Services.AddPurchaseInfrastructure(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("sales", new OpenApiInfo { Title = "Modulo Ventas", Version = "v1" });
    
    c.SwaggerDoc("inventory", new OpenApiInfo { Title = "Módulo de Inventario", Version = "v1" }); 
    
    c.SwaggerDoc("purchases", new OpenApiInfo { Title = "Módulo de Compra", Version = "v1" }); 
});

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

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/sales/swagger.json", "Modulo Ventas");
    
    c.SwaggerEndpoint("/swagger/inventory/swagger.json", "Modulo de Inventario");
    
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
app.MapControllers();
app.Run();