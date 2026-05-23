// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Shared.Application.Interfaces;
// using Sales.Application.Features.Tickets;
//
// namespace Backend.API.Controllers.Modules.Sales;
//
// [ApiController]
// [ApiExplorerSettings(GroupName = "sales")]
// [Route("api/sales/companies/{companyCen}/tickets")]
// public class TicketsController : ControllerBase
// {
//     private readonly IMediator _mediator;
//     private readonly ICurrentCompanyProvider _companyProvider;
//
//     public TicketsController(IMediator mediator, ICurrentCompanyProvider companyProvider)
//     {
//         _mediator = mediator;
//         _companyProvider = companyProvider;
//     }
//
//     [HttpGet]
//     public async Task<IActionResult> GetDailyTickets()
//     {
//         try
//         {
//             var tickets = await _mediator.Send(new GetDailyTicketsQuery(_companyProvider.CompanyId));
//             return Ok(tickets);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPost]
//     public async Task<IActionResult> CreateTicket([FromBody] CreateTicketContractRequest request)
//     {
//         try
//         {
//             var command = new CreateTicketCommand(_companyProvider.CompanyId, request.WaiterCen);
//             var ticket = await _mediator.Send(command);
//             return StatusCode(201, ticket);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpGet("{ticketCen}")]
//     public async Task<IActionResult> GetTicketByCen(string ticketCen)
//     {
//         try
//         {
//             var ticket = await _mediator.Send(new GetTicketByCenQuery(_companyProvider.CompanyId, ticketCen));
//             if (ticket == null) return NotFound();
//             return Ok(ticket);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpGet("{ticketCen}/items")]
//     public async Task<IActionResult> GetTicketItems(string ticketCen)
//     {
//         try
//         {
//             var items = await _mediator.Send(new GetTicketItemsQuery(_companyProvider.CompanyId, ticketCen));
//             return Ok(items);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPost("{ticketCen}/items")]
//     public async Task<IActionResult> AddItemToTicket(string ticketCen, [FromBody] CreateTicketItemContractRequest request)
//     {
//         try
//         {
//             var companyCen = RouteData.Values["companyCen"]?.ToString() ?? "";
//             var item = await _mediator.Send(new AddItemToTicketCommand(_companyProvider.CompanyId, companyCen, ticketCen, request));
//             if (item == null) return NotFound();
//             return StatusCode(201, item);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPatch("{ticketCen}/items/{ticketItemCen}")]
//     public async Task<IActionResult> UpdateTicketItem(string ticketCen, string ticketItemCen, [FromBody] UpdateTicketItemContractRequest request)
//     {
//         try
//         {
//             var item = await _mediator.Send(new UpdateTicketItemCommand(_companyProvider.CompanyId, ticketCen, ticketItemCen, request));
//             if (item == null) return NotFound();
//             return Ok(item);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPut("{ticketCen}/waiter")]
//     public async Task<IActionResult> AssignWaiter(string ticketCen, [FromBody] AssignTicketWaiterContractRequest request)
//     {
//         try
//         {
//             var response = await _mediator.Send(new AssignWaiterCommand(_companyProvider.CompanyId, ticketCen, request));
//             if (response == null) return NotFound();
//             return Ok(response);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPost("{ticketCen}/payment")]
//     public async Task<IActionResult> PayTicket(string ticketCen, [FromBody] PayTicketContractRequest request)
//     {
//         try
//         {
//             var companyCen = RouteData.Values["companyCen"]?.ToString() ?? "";
//             var response = await _mediator.Send(new PayTicketCommand(_companyProvider.CompanyId, companyCen, ticketCen, request));
//             if (response == null) return NotFound();
//             return Ok(response);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPost("{ticketCen}/cancel")]
//     public async Task<IActionResult> CancelTicket(string ticketCen, [FromBody] CancelTicketContractRequest? request)
//     {
//         try
//         {
//             var response = await _mediator.Send(new CancelTicketCommand(_companyProvider.CompanyId, ticketCen, request));
//             if (response == null) return NotFound();
//             return Ok(response);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpPost("{ticketCen}/send")]
//     public async Task<IActionResult> SendToKds(string ticketCen)
//     {
//         try
//         {
//             var items = await _mediator.Send(new SendToKdsCommand(_companyProvider.CompanyId, ticketCen));
//             if (items == null) return NotFound();
//             return Ok(items);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
//
//     [HttpGet("{ticketCen}/totals")]
//     public async Task<IActionResult> GetTicketTotals(string ticketCen)
//     {
//         try
//         {
//             var totals = await _mediator.Send(new GetTicketTotalsQuery(_companyProvider.CompanyId, ticketCen));
//             if (totals == null) return NotFound();
//             return Ok(totals);
//         }
//         catch (Exception ex)
//         {
//             return StatusCode(500, new { error = ex.Message });
//         }
//     }
// }