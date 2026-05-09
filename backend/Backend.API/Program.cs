using Backend.API.Filters;
using Microsoft.EntityFrameworkCore;
using Inventory.Application.Services;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Repositories;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;
using Shared.Application.Interfaces;
using Shared.Infrastructure;
using Shared.Infrastructure.Providers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
    );

Action<DbContextOptionsBuilder> npgsqlOptions = options => 
    options.UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention();


builder.Services.AddDbContext<InventoryDbContext>(npgsqlOptions);
builder.Services.AddDbContext<CoreDbContext>(npgsqlOptions);
builder.Services.AddDbContext<SalesDbContext>(npgsqlOptions);

builder.Services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();
builder.Services.AddScoped<IAdjustmentService, AdjustmentService>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<IGetProductKardexRepository, GetProductKardexRepository>();
builder.Services.AddScoped<IGetProductKardexService, GetProductKardexService>();

builder.Services.AddScoped<IGetStockRepository, GetStockRepository>();
builder.Services.AddScoped<IGetStockService, GetStockService>();

builder.Services.AddScoped<ILookUpRepository, LookUpRepository>();
builder.Services.AddScoped<ILookUpService, LookUpService>();

builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IMovementService, MovementService>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUnitService, UnitService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Sales.Application.Features.Tickets.CreateTicket.CreateTicketCommandHandler).Assembly);
});

builder.Services.AddScoped<ICurrentCompanyProvider, CurrentCompanyProvider>();

builder.Services.AddControllers(options => 
{
    options.Filters.Add<CompanyTenantFilter>();
});
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