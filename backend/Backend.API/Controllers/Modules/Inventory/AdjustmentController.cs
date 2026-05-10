using Microsoft.AspNetCore.Mvc;

using Inventory.Domain.Interfaces.IServices;
using Inventory.Domain.DTOs;
using Shared.Application.Interfaces;


namespace Backend.API.Controllers.Modules.Inventory;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/stock/adjustments")]
public class AdjustmentController: Controller
{
    private readonly IAdjustmentService  _inventoryService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public AdjustmentController(IAdjustmentService inventoryService, ICurrentCompanyProvider companyProvider)
    {
        _inventoryService = inventoryService;
        _companyProvider = companyProvider;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAjustment([FromBody] AdjustmentDto request)
    {
        try
        {
            int companyId = _companyProvider.CompanyId;

            await _inventoryService.RegisterAdjustmentAsync(companyId, request);

            return Ok(new { message = "Ajuste registrado correctamente. Ajuste realizado" });
        }
        catch (Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }
}