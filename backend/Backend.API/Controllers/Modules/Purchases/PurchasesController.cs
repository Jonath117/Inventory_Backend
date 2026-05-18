using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.Purchases.CreatePurchase;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Purchases;

[ApiController]
[ApiExplorerSettings(GroupName = "purchases")]
[Route("api/purchases/companies/{companyCen}/orders")]
public class PurchasesController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ICurrentCompanyProvider _companyProvider;

    public PurchasesController(ISender mediator, ICurrentCompanyProvider companyProvider)
    {
        _mediator = mediator;
        _companyProvider = companyProvider;
    }


    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder(
        [FromRoute] string companyCen, 
        [FromBody] CreatePurchaseRequestDto request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;

            var command = new CreatePurchaseCommand(
                companyId,
                request.SupplierCen,
                request.WarehouseCen,
                request.Items.Select(i => new CreatePurchaseItemDto(i.ProductCen, i.Quantity)).ToList()
            );

            var result = await _mediator.Send(command);

            return StatusCode(201, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creando orden de compra: {ex}");
            return StatusCode(500, new { error = "Ocurrio un error interno en el servidor." });
        }
    }
}

