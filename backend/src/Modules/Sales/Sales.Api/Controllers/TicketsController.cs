using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;
using Sales.Application.Features.Tickets;

namespace Sales.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/companies/{companyCen}/tickets")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public TicketsController(IMediator mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetDailyTickets(string companyCen)
    {
        var tickets = await _mediator.Send(new GetDailyTicketsQuery(_companyProvider.CompanyId));
        return Ok(tickets);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(string companyCen, [FromBody] CreateTicketContractRequest request) 
    {
        var command = new CreateTicketCommand(_companyProvider.CompanyId, request.WaiterCen, request.WarehouseCen);
        var ticket = await _mediator.Send(command);
        return StatusCode(201, ticket);
    }

    [HttpGet("{ticketCen}")]
    public async Task<IActionResult> GetTicketByCen(string companyCen, string ticketCen) 
    {
        var ticket = await _mediator.Send(new GetTicketByCenQuery(_companyProvider.CompanyId, ticketCen));
        return Ok(ticket);
    }

    [HttpGet("{ticketCen}/items")]
    public async Task<IActionResult> GetTicketItems(string companyCen, string ticketCen) 
    {
        var items = await _mediator.Send(new GetTicketItemsQuery(_companyProvider.CompanyId, ticketCen));
        return Ok(items);
    }

    [HttpPost("{ticketCen}/items")]
    public async Task<IActionResult> AddItemToTicket(string companyCen, string ticketCen, [FromBody] CreateTicketItemContractRequest request)
    {
        var companyId = _companyProvider.CompanyId;
        var item = await _mediator.Send(new AddItemToTicketCommand(companyId, companyCen, ticketCen, request));
        return StatusCode(201, item);
    }

    [HttpPatch("{ticketCen}/items/{ticketItemCen}")]
    public async Task<IActionResult> UpdateTicketItem(string companyCen, string ticketCen, string ticketItemCen, [FromBody] UpdateTicketItemContractRequest request) 
    {
        var item = await _mediator.Send(new UpdateTicketItemCommand(_companyProvider.CompanyId, ticketCen, ticketItemCen, request));
        return Ok(item);
    }

    [HttpPut("{ticketCen}/waiter")]
    public async Task<IActionResult> AssignWaiter(string companyCen, string ticketCen, [FromBody] AssignTicketWaiterContractRequest request) 
    {
        var response = await _mediator.Send(new AssignWaiterCommand(_companyProvider.CompanyId, ticketCen, request));
        return Ok(response);
    }

    [HttpPost("{ticketCen}/payment")]
    public async Task<IActionResult> PayTicket(string companyCen, string ticketCen, [FromBody] PayTicketContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var response = await _mediator.Send(new PayTicketCommand(companyId, companyCen, ticketCen, request));
        return Ok(response);
    }

    [HttpPost("{ticketCen}/cancel")]
    public async Task<IActionResult> CancelTicket(string companyCen, string ticketCen, [FromBody] CancelTicketContractRequest? request) 
    {
        var response = await _mediator.Send(new CancelTicketCommand(_companyProvider.CompanyId, ticketCen, request));
        return Ok(response);
    }

    [HttpPost("{ticketCen}/send")]
    public async Task<IActionResult> SendToKds(string companyCen, string ticketCen) 
    {
        var items = await _mediator.Send(new SendToKdsCommand(_companyProvider.CompanyId, ticketCen));
        return Ok(items);
    }

    [HttpGet("{ticketCen}/totals")]
    public async Task<IActionResult> GetTicketTotals(string companyCen, string ticketCen) 
    {
        var totals = await _mediator.Send(new GetTicketTotalsQuery(_companyProvider.CompanyId, ticketCen));
        return Ok(totals);
    }

    [HttpPost("{ticketCen}/items/{ticketItemCen}/resend")]
    public async Task<IActionResult> ResendTicketItem(string companyCen, string ticketCen, string ticketItemCen) 
    {
        // TODO: Implement logic according to requirements
        return Ok();
    }

    [HttpGet("{ticketCen}/print")]
    public async Task<IActionResult> PrintTicket(string companyCen, string ticketCen) 
    {
        // TODO: Implement logic according to requirements
        return Ok();
    }
}