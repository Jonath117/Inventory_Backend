using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using Sales.Application.Features.TaxConfiguration;
using Sales.Application.Features.Tickets;
using Sales.Application.Features.TaxConfiguration.UpdateTaxConfiguration;

namespace Sales.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/companies/{companyCen}/tax-configuration")]
public class TaxConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public TaxConfigurationController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaxConfiguration()
    {
        var config = await _mediator.Send(new GetTaxConfigurationQuery(_companyProvider.CompanyId));
        return Ok(config);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTaxConfiguration([FromBody] UpdateTaxConfigurationContractRequest request)
    {
        var config = await _mediator.Send(new UpdateTaxConfigurationCommand(_companyProvider.CompanyId, request));
        return Ok(config);
    }
}