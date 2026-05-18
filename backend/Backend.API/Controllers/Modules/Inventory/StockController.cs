using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IServices;
using Shared.Application.Interfaces;

namespace Backend.API.Controllers.Modules.Inventory;

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
        try
        {
            int companyId = _companyProvider.CompanyId;
            var response = await _movementService.ConsumeStockAsync(companyId, request);
            
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error consumiendo stock: {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }
    
    [HttpPost("increase")]
    public async Task<IActionResult> IncreaseStock([FromBody] StockIncreaseContractRequest request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var documentCen = await _movementService.IncreaseStockAsync(companyId, request);
            
            return Ok(documentCen);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error incrementando stock: {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }
    
    [HttpPost("adjustments")]
    public async Task<IActionResult> CreateAdjustment([FromBody] InventoryAdjustmentContractRequest request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var response = await _adjustmentService.RegisterAdjustmentAsync(companyId, request);
            
            return StatusCode(201, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Si el ajuste deja el stock en negativo, lanzamos un 409 Conflict
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registrando ajuste: {ex}");
            return StatusCode(500, new { error = "Ocurrió un error interno en el servidor." });
        }
    }
}