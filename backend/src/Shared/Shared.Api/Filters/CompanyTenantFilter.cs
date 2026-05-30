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
        
        if (context.RouteData.Values.TryGetValue("companyCen", out var cenValue) && cenValue is string companyCen)
        {
            var company = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyCen == companyCen);

            if (company == null)
            {
                context.Result = new NotFoundObjectResult(new { error = $"La empresa con código {companyCen} no existe." });
                return; 
            }
            
            finalCompanyId = company.Id; 
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