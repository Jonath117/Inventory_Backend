using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using Sales.Application.Features.Dashboard;

namespace Sales.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/companies/{companyCen}/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public DashboardController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpGet("daily-sales")]
    public async Task<IActionResult> GetDailySales()
    {
        var dashboard = await _mediator.Send(new GetDailySalesDashboardQuery(_companyProvider.CompanyId));
        return Ok(dashboard);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> GetTopProducts([FromQuery] int topN = 10)
    {
        var topProducts = await _mediator.Send(new GetTopProductsQuery(_companyProvider.CompanyId, topN));
        return Ok(topProducts);
    }

    [HttpGet("kds-status")]
    public async Task<IActionResult> GetKdsStatus()
    {
        var status = await _mediator.Send(new GetKdsStatusQuery(_companyProvider.CompanyId));
        return Ok(status);
    }
}