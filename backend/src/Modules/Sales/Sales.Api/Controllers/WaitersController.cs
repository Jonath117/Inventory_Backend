using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using Sales.Application.Features.Waiters;

namespace Sales.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/companies/{companyCen}/waiters")]
public class WaitersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public WaitersController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetWaiters()
    {
        var waiters = await _mediator.Send(new GetWaitersQuery(_companyProvider.CompanyId));
        return Ok(waiters);
    }
}