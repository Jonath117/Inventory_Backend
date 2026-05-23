using Inventory.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Interfaces;

namespace Inventory.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "inventory")]
[Route("api/inventory/companies/{companyCen}/warehouses")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    private readonly ICurrentCompanyProvider _companyProvider;

    public WarehouseController(IWarehouseService warehouseService, ICurrentCompanyProvider companyProvider)
    {
        _warehouseService = warehouseService;
        _companyProvider = companyProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetWarehouses()
    {
        try
        {
            int companyId = _companyProvider.CompanyId;
            var warehouses = await _warehouseService.GetWarehousesAsync(companyId);
            return Ok(warehouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}