using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using Sales.Application.Features.Catalog;

namespace Backend.API.Controllers.Modules.Sales;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/companies/{companyCen}/catalog/products")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public CatalogController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetSellableProducts(
        [FromQuery] string? search, 
        [FromQuery] string? categoryCen, 
        [FromQuery] string? warehouseCen, 
        [FromQuery] bool onlyAvailable = true, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        var companyCen = RouteData.Values["companyCen"]?.ToString() ?? "";
        var query = new GetSellableProductsQuery(_companyProvider.CompanyId, companyCen, search, categoryCen, warehouseCen, onlyAvailable, page, pageSize);
        var products = await _mediator.Send(query);
        return Ok(products);
    }
}