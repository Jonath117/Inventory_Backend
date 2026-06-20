using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/stock")]
public class StockController : ControllerBase
{
    private readonly IMovementService _movementService;
    private readonly IAdjustmentService _adjustmentService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public StockController(
        IMovementService movementService, 
        IAdjustmentService adjustmentService,
        ICurrentCompanyProvider companyProvider)
    {
        _movementService = movementService;
        _adjustmentService = adjustmentService;
        _companyProvider = companyProvider;
    }
    
    [HttpPost("consume")]
    public async Task<IActionResult> ConsumeStock([FromBody] StockConsumeContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var response = await _movementService.ConsumeStockAsync(companyId, request);
        return Ok(response);
    }
    
    [HttpPost("increase")]
    public async Task<IActionResult> IncreaseStock(
        [FromServices] System.Threading.Channels.Channel<RestockEvent> restockChannel,
        [FromServices] IProductService productService,
        [FromBody] StockIncreaseContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var documentCen = await _movementService.IncreaseStockAsync(companyId, request);
        
        foreach (var item in request.Items)
        {
            string productName = item.ProductCen;
            try 
            {
                var prod = await productService.GetProductDetailsByCenAsync(companyId, item.ProductCen);
                if (prod != null && !string.IsNullOrEmpty(prod.Name)) 
                {
                    productName = prod.Name;
                }
            }
            catch 
            {
                // Fallback to CEN if product name cannot be fetched
            }

            await restockChannel.Writer.WriteAsync(new RestockEvent 
            { 
                Producto = productName, 
                Cantidad = item.Quantity 
            });
        }
        
        return Ok(documentCen);
    }
    
    [HttpPost("adjustments")]
    public async Task<IActionResult> CreateAdjustment([FromBody] InventoryAdjustmentContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var response = await _adjustmentService.RegisterAdjustmentAsync(companyId, request);
        return StatusCode(201, response);
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateStock([FromBody] StockValidationContractRequest request)
    {
        int companyId = _companyProvider.CompanyId;
        var response = await _movementService.ValidateStockAsync(companyId, request);
        return Ok(response);
    }
}