// using MediatR;
// using Microsoft.AspNetCore.Mvc;
// using Shared.Application.Interfaces;
// using Sales.Application.Features.Kds;
// using Sales.Application.Features.Tickets;
// using Sales.Application.Features.Kds.UpdateKdsItemStatus;
//
// namespace Backend.API.Controllers.Modules.Sales;
//
// [ApiController]
// [ApiExplorerSettings(GroupName = "sales")]
// [Route("api/sales/companies/{companyCen}/kds")]
// public class KdsController : ControllerBase
// {
//     private readonly IMediator _mediator;
//     private readonly ICurrentCompanyProvider _companyProvider;
//
//     public KdsController(IMediator mediator, ICurrentCompanyProvider companyProvider)
//     {
//         _mediator = mediator;
//         _companyProvider = companyProvider;
//     }
//
//     [HttpGet("teams")]
//     public async Task<IActionResult> GetKdsTeams()
//     {
//         var teams = await _mediator.Send(new GetKdsTeamsQuery(_companyProvider.CompanyId));
//         return Ok(teams);
//     }
//
//     [HttpGet("teams/{teamCen}/items")]
//     public async Task<IActionResult> GetKdsItems(string teamCen)
//     {
//         var items = await _mediator.Send(new GetKdsItemsQuery(_companyProvider.CompanyId, teamCen));
//         return Ok(items);
//     }
//
//     [HttpPatch("items/{ticketItemCen}/status")]
//     public async Task<IActionResult> UpdateKdsItemStatus(string ticketItemCen, [FromBody] UpdateKdsItemStatusContractRequest request)
//     {
//         var success = await _mediator.Send(new UpdateKdsItemStatusCommand(_companyProvider.CompanyId, ticketItemCen, request));
//         if (!success) return NotFound();
//         return Ok();
//     }
// }