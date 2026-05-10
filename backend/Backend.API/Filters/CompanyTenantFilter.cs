using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Infrastructure;

namespace Backend.API.Filters;

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
        int finalCompanyId = 0;

        if (context.RouteData.Values.TryGetValue("companyCen", out var cenValue) && cenValue is string companyCen)
        {
            var company = await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Cen == companyCen);

            if (company == null)
            {
                context.Result = new NotFoundObjectResult(new { message = $"La empresa con codigo {companyCen} no existe." });
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
            }
            finalCompanyId = headerCompanyId;
        }
        else
        {
            context.Result = new  NotFoundObjectResult(new { message = $"Se requiere identificar a la empresa" });
        }
        
        _currentCompanyProvider.SetCompanyId(finalCompanyId);
        
        await next();
    }
}