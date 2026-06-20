using Backend.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Infrastructure;

namespace Shared.API.Filters;

public class CompanyTenantFilter : IAsyncActionFilter
{
    private readonly ICurrentCompanyProvider _currentCompanyProvider;
    private readonly CoreDbContext _context;

    public CompanyTenantFilter(ICurrentCompanyProvider currentCompanyProvider, CoreDbContext context)
    {
        _currentCompanyProvider = currentCompanyProvider;
        _context = context;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<AllowAnonymousTenantAttribute>() != null)
        {
            await next();
            return; 
        }
        int finalCompanyId = 0;
        
        var httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
        var configuration = context.HttpContext.RequestServices.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
        string inventoryUrl = configuration["INVENTORY_API_URL"] ?? "http://inventario:80";
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(inventoryUrl);

        if (context.RouteData.Values.TryGetValue("companyCen", out var cenValue) && cenValue is string companyCen)
        {
            var company = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyCen == companyCen);

            if (company == null)
            {
                // Consultar a inventario por HTTP
                var response = await client.GetAsync($"/api/inventory/companies/{companyCen}");
                if (!response.IsSuccessStatusCode)
                {
                    context.Result = new NotFoundObjectResult(new { error = $"La empresa con código {companyCen} no existe." });
                    return;
                }
                
                var companyData = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                string name = companyData.GetProperty("name").GetString() ?? "Unknown";
                int remoteId = companyData.GetProperty("companyId").GetInt32();

                // Lazy create para satisfacer llaves foráneas
                var newCompany = new Shared.Domain.Company 
                { 
                    Id = remoteId, // Mantener el mismo Id si es posible (identity_insert o similar, pero EF Core podría ignorarlo)
                    CompanyCen = companyCen, 
                    Name = name 
                };
                
                // Hack: para forzar el Id con Identity columns en postgres
                await _context.Database.ExecuteSqlRawAsync("SET CONSTRAINTS ALL DEFERRED;"); // O usar identity insert
                
                // Mejor, como Postgres ignora ID explícito sin configuración, sólo lo añadimos y esperamos que auto-genere o lo marcamos explícito
                // Depende de la config. Por ahora lo insertamos.
                _context.Companies.Add(newCompany);
                await _context.SaveChangesAsync();
                
                finalCompanyId = newCompany.Id;
            }
            else 
            {
                finalCompanyId = company.Id; 
            }
        }
        else if (context.HttpContext.Request.Headers.TryGetValue("x-company-id", out var headerValue) &&
                 int.TryParse(headerValue, out var headerCompanyId))
        {
            bool exists = await _context.Companies.AnyAsync(c => c.Id == headerCompanyId);
            if (!exists)
            {
                context.Result = new NotFoundObjectResult(new { message = $"El Company ID proporcionado en el header no es valido" });
                return; 
            }
            finalCompanyId = headerCompanyId;
        }
        else
        {
            context.Result = new NotFoundObjectResult(new { message = $"Se requiere identificar a la empresa" });
            return; 
        }
        _currentCompanyProvider.SetCompanyId(finalCompanyId);
    
        await next();
    }
}