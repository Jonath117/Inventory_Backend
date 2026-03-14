using Microsoft.AspNetCore.Mvc;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Interfaces.IServices;

namespace Backend.API.Modules.Inventory;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromHeader(Name = "x-company-id")] int companyId)
    {
        try
        {
            if (companyId <= 0)
                return BadRequest("Enviar el header 'x-company-id' con id valido");

            var dashboard = await _inventoryService.GetDashboardMetricsAsync(companyId);

            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            return BadRequest(new {error = ex.Message});
        }
    }
    

}