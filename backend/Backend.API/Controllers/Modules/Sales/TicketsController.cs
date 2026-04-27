using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Features.Tickets.CreateTicket;

namespace Backend.API.Controllers.Modules.Sales;

[ApiController]
[Route("api/sales/[controller]")]
public class TicketsController: ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromHeader(Name = "x-company-id")] int companyId)
    {
        if (companyId <= 0)
        {
            return BadRequest(new { error = "Id de compañia Invalido" });
        }
        
        var command = new CreateTicketCommand(companyId);
        var ticketId = await _mediator.Send(command);
        
        return Created($"/api/sales/tickets/{companyId}", new {TicketId = ticketId});
    }
}