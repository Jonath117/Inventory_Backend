using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Features.Tickets.CreateTicket;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Sales;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/[controller]")]
public class TicketsController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public TicketsController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket()
    {
        int internalCompanyId = _companyProvider.CompanyId;
        
        var command = new CreateTicketCommand(internalCompanyId);
        var ticketId = await _mediator.Send(command);
          
        return Created($"/api/sales/tickets/{internalCompanyId}", new {TicketId = ticketId});
    }
}