using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.Purchases.ConfirmPurchase;
using Purchases.Application.Features.Purchases.CreatePurchase;
using Purchases.Application.Features.Purchases.GetSuppliers;
using Purchases.Application.Features.Purchases.GetByIdPurchase;
using Purchases.Application.Features.Purchases.GetPurchase;
using Shared.Application.Interfaces;

namespace Purchases.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "purchases")]
[Route("api/purchases/companies/{companyCen}")]
public class PurchasesController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public PurchasesController(ISender mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreatePurchaseOrder([FromRoute] string companyCen, [FromBody] CreatePurchaseRequestDto request)
    {
        try
        {
            var command = new CreatePurchaseCommand(
                _companyProvider.CompanyId,
                request.SupplierCen,
                request.WarehouseCen,
                request.Items.Select(i => new CreatePurchaseItemDto(i.ProductCen, i.Quantity)).ToList()
            );

            var result = await _mediator.Send(command);
            return StatusCode(201, result);
        }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetPurchaseOrders([FromQuery] int? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool sortDescending = true)
    {
        try
        {
            var query = new GetPurchaseOrdersQuery(_companyProvider.CompanyId, status, page, pageSize, sortDescending);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }

    [HttpGet("orders/{orderCen}")]
    public async Task<IActionResult> GetPurchaseOrderDetail([FromRoute] string orderCen)
    {
        try
        {
            var query = new GetPurchaseOrderDetailQuery(_companyProvider.CompanyId, orderCen);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }

    [HttpPost("orders/{orderCen}/confirm")]
    public async Task<IActionResult> ConfirmPurchaseOrder([FromRoute] string companyCen, [FromRoute] string orderCen)
    {
        try
        {
            var command = new ConfirmPurchaseOrderCommand(_companyProvider.CompanyId, companyCen, orderCen);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }

    [HttpGet("suppliers")]
    public async Task<IActionResult> GetSuppliers()
    {
        try
        {
            var query = new GetSuppliersQuery(_companyProvider.CompanyId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }
}